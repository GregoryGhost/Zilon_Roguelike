﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Zilon.Core.Client;
using Zilon.Core.Persons;
using Zilon.Core.Tactics;
using Zilon.Core.Tactics.Behaviour;
using Zilon.Core.Tactics.Spatial;

namespace Zilon.Core.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// Команда на перемещение взвода в указанный узел карты.
    /// </summary>
    public class AttackCommand : ActorCommandBase
    {
        private readonly ITacticalActUsageService _tacticalActUsageService;

        [ExcludeFromCodeCoverage]
        public AttackCommand(IGameLoop gameLoop,
            ISectorManager sectorManager,
            IPlayerState playerState,
            ITacticalActUsageService tacticalActUsageService) :
            base(gameLoop, sectorManager, playerState)
        {
            _tacticalActUsageService = tacticalActUsageService;
        }

        public override bool CanExecute()
        {
            var map = SectorManager.CurrentSector.Map;

            var currentNode = PlayerState.ActiveActor.Actor.Node;

            var selectedActorViewModel = GetSelectedActorViewModel();
            if (selectedActorViewModel == null)
            {
                return false;
            }

            var targetNode = selectedActorViewModel.Actor.Node;

            var targetIsOnLine = MapHelper.CheckNodeAvailability(map, currentNode, targetNode);
            var act = PlayerState.ActiveActor.Actor.Person.TacticalActCarrier.Acts.FirstOrDefault();
            var isInDistance = act.CheckDistance(((HexNode)currentNode).CubeCoords, ((HexNode)targetNode).CubeCoords);

            var canExecute = targetIsOnLine && isInDistance;

            //TODO Добавить проверку:
            // 1. Выбран ли вражеский юнит.
            // 2. Находится ли в пределах досягаемости оружия. (1) Проверяется.
            // 3. Видим ли текущим актёром. (0.5) Условная видимость по прямой.
            // 4. Способно ли оружие атаковать.
            // 5. Доступен ли целевой актёр для атаки.
            // 6. Возможно ли выполнение каких-либо команд над актёрами
            // (Нельзя, если ещё выполняется текущая команда. Например, анимация перемещения.)
            return canExecute;
        }

        private IActorViewModel GetSelectedActorViewModel()
        {
            return PlayerState.HoverViewModel as IActorViewModel;
        }

        protected override void ExecuteTacticCommand()
        {
            var targetActorViewModel = (IActorViewModel)PlayerState.HoverViewModel;

            var targetActor = targetActorViewModel.Actor;
            var intention = new Intention<AttackTask>(a => new AttackTask(a, targetActor, _tacticalActUsageService));
            PlayerState.TaskSource.Intent(intention);
        }
    }
}