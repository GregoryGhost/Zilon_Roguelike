﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Zilon.Bot.Players;
using Zilon.Bot.Sdk;
using Zilon.Core.Persons;
using Zilon.Core.Players;
using Zilon.Core.Props;
using Zilon.Core.Schemes;
using Zilon.Core.Tactics;

namespace Zilon.Emulation.Common
{
    public abstract class AutoplayEngineBase<T> where T : IPluggableActorTaskSource
    {
        private const int ITERATION_LIMIT = 40_000;

        private bool _changeSector;
        protected readonly BotSettings _botSettings;

        protected IServiceScope ServiceScope { get; set; }

        public AutoplayEngineBase(BotSettings botSettings)
        {
            _botSettings = botSettings;
        }

        public async Task StartAsync(IServiceProvider _globalServiceProvider)
        {
            if (_globalServiceProvider is null)
            {
                throw new ArgumentNullException(nameof(_globalServiceProvider));
            }

            var humanActor = await CreateSectorAsync(_globalServiceProvider).ConfigureAwait(false);
            var gameLoop = ServiceScope.ServiceProvider.GetRequiredService<IGameLoop>();
            var botActorTaskSource = _globalServiceProvider.GetRequiredService<T>();
            botActorTaskSource.Configure(_botSettings);

            var iterationCounter = 1;
            while (!humanActor.Person.Survival.IsDead && iterationCounter <= ITERATION_LIMIT)
            {
                try
                {
                    gameLoop.Update();

                    if (_changeSector)
                    {
                        humanActor = await CreateSectorAsync(_globalServiceProvider).ConfigureAwait(false);

                        gameLoop = ServiceScope.ServiceProvider.GetRequiredService<IGameLoop>();
                        botActorTaskSource = ServiceScope.ServiceProvider.GetRequiredService<T>();
                        botActorTaskSource.Configure(_botSettings);

                        _changeSector = false;
                    }
                }
                catch (ActorTaskExecutionException exception)
                {
                    CatchActorTaskExecutionException(exception);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch(Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    CatchException(exception);
                }
            };

            ProcessEnd();
        }

        protected abstract void CatchException(Exception exception);

        protected abstract void CatchActorTaskExecutionException(ActorTaskExecutionException exception);

        protected abstract void ProcessEnd();

        private static IActor CreateHumanActor(HumanPlayer humanPlayer,
            ISchemeService schemeService,
            ISurvivalRandomSource survivalRandomSource,
            IPropFactory propFactory,
            ISectorManager sectorManager,
            IActorManager actorManager)
        {
            var personScheme = schemeService.GetScheme<IPersonScheme>("human-person");

            var playerActorStartNode = sectorManager.CurrentSector.Map.Regions
                .SingleOrDefault(x => x.IsStart)
                .Nodes
                .First();

            if (humanPlayer.MainPerson == null)
            {
                var inventory = new Inventory();

                var evolutionData = new EvolutionData(schemeService);

                var defaultActScheme = schemeService.GetScheme<ITacticalActScheme>(personScheme.DefaultAct);

                var person = new HumanPerson(personScheme, defaultActScheme, evolutionData, survivalRandomSource, inventory);

                humanPlayer.MainPerson = person;

                // TODO Использовать генератор персонажа, как в игре.
                // Для этого нужно научить ботов корректно использовать оружие дальнего боя и посохи лечения.
                var classRoll = new Random().Next(1, 3);
                switch (classRoll)
                {
                    case 1:
                        AddEquipmentToActor(person.EquipmentCarrier, 2, "short-sword", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 1, "steel-armor", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 3, "wooden-shield", schemeService, propFactory);
                        break;

                    case 2:
                        AddEquipmentToActor(person.EquipmentCarrier, 2, "battle-axe", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 3, "battle-axe", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 0, "highlander-helmet", schemeService, propFactory);
                        break;

                    case 3:
                        AddEquipmentToActor(person.EquipmentCarrier, 2, "bow", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 1, "leather-jacket", schemeService, propFactory);
                        AddEquipmentToActor(inventory, "short-sword", schemeService, propFactory);
                        AddResourceToActor(inventory, "arrow", 10, schemeService, propFactory);
                        break;

                    case 4:
                        AddEquipmentToActor(person.EquipmentCarrier, 2, "fireball-staff", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 1, "scholar-robe", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 0, "wizard-hat", schemeService, propFactory);
                        AddResourceToActor(inventory, "mana", 15, schemeService, propFactory);
                        break;

                    case 5:
                        AddEquipmentToActor(person.EquipmentCarrier, 2, "pistol", schemeService, propFactory);
                        AddEquipmentToActor(person.EquipmentCarrier, 0, "elder-hat", schemeService, propFactory);
                        AddResourceToActor(inventory, "bullet-45", 5, schemeService, propFactory);

                        AddResourceToActor(inventory, "packed-food", 1, schemeService, propFactory);
                        AddResourceToActor(inventory, "water-bottle", 1, schemeService, propFactory);
                        AddResourceToActor(inventory, "med-kit", 1, schemeService, propFactory);

                        AddResourceToActor(inventory, "mana", 5, schemeService, propFactory);
                        AddResourceToActor(inventory, "arrow", 3, schemeService, propFactory);
                        break;

                    default:
                        throw new InvalidOperationException("Эта комбинация начальной экипировки не поддерживается.");
                }

                AddResourceToActor(inventory, "packed-food", 1, schemeService, propFactory);
                AddResourceToActor(inventory, "water-bottle", 1, schemeService, propFactory);
                AddResourceToActor(inventory, "med-kit", 1, schemeService, propFactory);
            }

            var actor = new Actor(humanPlayer.MainPerson, humanPlayer, playerActorStartNode);

            actorManager.Add(actor);

            return actor;
        }

        private static void AddEquipmentToActor(IEquipmentCarrier equipmentCarrier, int slotIndex, string equipmentSid,
            ISchemeService schemeService, IPropFactory propFactory)
        {
            try
            {
                var equipmentScheme = schemeService.GetScheme<IPropScheme>(equipmentSid);
                var equipment = propFactory.CreateEquipment(equipmentScheme);
                equipmentCarrier[slotIndex] = equipment;
            }
            catch (KeyNotFoundException)
            {
                Debug.WriteLine($"Не найден объект {equipmentSid}");
            }
        }

        private static void AddEquipmentToActor(Inventory inventory, string equipmentSid,
            ISchemeService schemeService, IPropFactory propFactory)
        {
            try
            {
                var equipmentScheme = schemeService.GetScheme<IPropScheme>(equipmentSid);
                var equipment = propFactory.CreateEquipment(equipmentScheme);
                inventory.Add(equipment);
            }
            catch (KeyNotFoundException)
            {
                Debug.WriteLine($"Не найден объект {equipmentSid}");
            }
        }

        private static void AddResourceToActor(Inventory inventory, string resourceSid, int count,
            ISchemeService schemeService, IPropFactory propFactory)
        {
            try
            {
                var resourceScheme = schemeService.GetScheme<IPropScheme>(resourceSid);
                var resource = propFactory.CreateResource(resourceScheme, count);
                inventory.Add(resource);
            }
            catch (KeyNotFoundException)
            {
                Debug.WriteLine($"Не найден объект {resourceSid}");
            }
        }


        private async Task<IActor> CreateSectorAsync(IServiceProvider _globalServiceProvider)
        {
            if (ServiceScope != null)
            {
                ServiceScope.Dispose();
                ServiceScope = null;
            }

            ServiceScope = _globalServiceProvider.CreateScope();

            ConfigBotAux();

            var schemeService = _globalServiceProvider.GetRequiredService<ISchemeService>();
            var humanPlayer = _globalServiceProvider.GetRequiredService<HumanPlayer>();
            var survivalRandomSource = _globalServiceProvider.GetRequiredService<ISurvivalRandomSource>();
            var propFactory = _globalServiceProvider.GetRequiredService<IPropFactory>();
            var scoreManager = _globalServiceProvider.GetRequiredService<IScoreManager>();

            var gameLoop = ServiceScope.ServiceProvider.GetRequiredService<IGameLoop>();
            var sectorManager = ServiceScope.ServiceProvider.GetRequiredService<ISectorManager>();
            var botActorTaskSource = ServiceScope.ServiceProvider.GetRequiredService<T>();
            var actorManager = ServiceScope.ServiceProvider.GetRequiredService<IActorManager>();
            var monsterActorTaskSource = ServiceScope.ServiceProvider.GetRequiredService<MonsterBotActorTaskSource>();

            await sectorManager.CreateSectorAsync().ConfigureAwait(false);

            sectorManager.CurrentSector.ScoreManager = scoreManager;
            sectorManager.CurrentSector.HumanGroupExit += CurrentSector_HumanGroupExit;

            gameLoop.ActorTaskSources = new Core.Tactics.Behaviour.IActorTaskSource[] {
                botActorTaskSource,
                monsterActorTaskSource
            };

            var humanActor = CreateHumanActor(humanPlayer,
                schemeService,
                survivalRandomSource,
                propFactory,
                sectorManager,
                actorManager);

            return humanActor;
        }

        protected abstract void ConfigBotAux();

        private void CurrentSector_HumanGroupExit(object sender, SectorExitEventArgs e)
        {
            Console.WriteLine("Exit");
            _changeSector = true;

            var sectorManager = ServiceScope.ServiceProvider.GetRequiredService<ISectorManager>();
            sectorManager.CurrentSector.HumanGroupExit -= CurrentSector_HumanGroupExit;
        }
    }
}
