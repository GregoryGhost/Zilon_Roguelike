using Assets.Zilon.Scripts.Services;

using Zenject;

using Zilon.Core.Client;
using Zilon.Core.Commands;
using Zilon.Core.MapGenerators;
using Zilon.Core.MapGenerators.RoomStyle;
using Zilon.Core.Persons;
using Zilon.Core.Tactics;
using Zilon.Core.Tactics.Behaviour;
using Zilon.Core.Tactics.Behaviour.Bots;

public class SectorInstaller : MonoInstaller<SectorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IGameLoop>().To<GameLoop>().AsSingle();
        Container.Bind<ICommandManager>().To<QueueCommandManager>().AsSingle();
        Container.Bind<IPlayerState>().To<PlayerState>().AsSingle();
        Container.Bind<IActorManager>().To<ActorManager>().AsSingle();
        Container.Bind<IPropContainerManager>().To<PropContainerManager>().AsSingle();
        Container.Bind<ITraderManager>().To<TraderManager>().AsSingle();
        Container.Bind<IHumanActorTaskSource>().To<HumanActorTaskSource>().AsSingle();
        Container.Bind<IActorTaskSource>().WithId("monster").To<MonsterActorTaskSource>().AsSingle();
        Container.Bind<ITacticalActUsageService>().To<TacticalActUsageService>().AsSingle();
        Container.Bind<ITacticalActUsageRandomSource>().To<TacticalActUsageRandomSource>().AsSingle();
        Container.Bind<ISurvivalRandomSource>().To<SurvivalRandomSource>().AsSingle();

        Container.Bind<ISectorManager>().To<SectorManager>().AsSingle();
        Container.Bind<ISectorModalManager>().FromInstance(GetSectorModalManager()).AsSingle();

        // ��������� �������
        Container.Bind<ISectorGeneratorSelector>().To<SectorGeneratorSelector>().AsSingle();
        Container.Bind<IDungeonSectorGenerator>().To<SectorProceduralGenerator>().AsSingle();
        Container.Bind<ITownSectorGenerator>().To<TownSectorGenerator>().AsSingle();
        Container.Bind<IMapFactory>().To<RoomMapFactory>().AsSingle().WhenInjectedInto<SectorProceduralGenerator>();
        Container.Bind<IMapFactory>().To<TownMapFactory>().AsSingle().WhenInjectedInto<TownSectorGenerator>();
        Container.Bind<IRoomGeneratorRandomSource>().To<RoomGeneratorRandomSource>().AsSingle();
        Container.Bind<IRoomGenerator>().To<RoomGenerator>().AsSingle();
        Container.Bind<IChestGenerator>().To<ChestGenerator>().AsSingle();
        Container.Bind<IChestGeneratorRandomSource>().To<ChestGeneratorRandomSource>().AsSingle();
        Container.Bind<IMonsterGenerator>().To<MonsterGenerator>().AsSingle();
        Container.Bind<IMonsterGeneratorRandomSource>().To<MonsterGeneratorRandomSource>().AsSingle();
        Container.Bind<ISectorFactory>().To<SectorFactory>().AsSingle();


        // ������������������ ������� ��� Ui.
        Container.Bind<IInventoryState>().To<InventoryState>().AsSingle();
        Container.Bind<ILogService>().To<LogService>().AsSingle();

        // �������� �����.
        Container.Bind<ICommand>().WithId("move-command").To<MoveCommand>().AsSingle();
        Container.Bind<ICommand>().WithId("attack-command").To<AttackCommand>().AsSingle();
        Container.Bind<ICommand>().WithId("open-container-command").To<OpenContainerCommand>().AsSingle();
        Container.Bind<ICommand>().WithId("next-turn-command").To<NextTurnCommand>().AsSingle();
        Container.Bind<ICommand>().WithId("use-self-command").To<UseSelfCommand>().AsSingle();

        // ������� ��� UI.
        Container.Bind<ICommand>().WithId("show-container-modal-command").To<ShowContainerModalCommand>().AsSingle();
        Container.Bind<ICommand>().WithId("show-inventory-command").To<ShowInventoryModalCommand>().AsSingle();
        Container.Bind<ICommand>().WithId("show-perks-command").To<ShowPerksModalCommand>().AsSingle();

        // ������������������ ������� ��� Ui.
        Container.Bind<ICommand>().WithId("equip-command").To<EquipCommand>().AsTransient();
        Container.Bind<ICommand>().WithId("prop-transfer-command").To<PropTransferCommand>().AsTransient();
    }


    private SectorModalManager GetSectorModalManager()
    {
        var sectorModalManager = FindObjectOfType<SectorModalManager>();
        return sectorModalManager;
    }
}