using Managment;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform playerShip;
    [SerializeField] private Factory factory;
    [SerializeField] private WorldBordersController worldBordersController;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<PlayerHitSignal>();
        Container.Bind<ConfigService>().AsSingle();
        Container.Bind<WorldBordersController>().FromInstance(worldBordersController).AsSingle();
        Container.Bind<MyObjectPool<IPoolable>>().AsSingle();
        Container.Bind<Transform>().FromInstance(playerShip).AsSingle();
        Container.Bind<Factory>().FromInstance(factory).AsSingle();

#if UNITY_IOS || UNITY_ANDROID
        Container.Bind<IInputService>()
            .To<MobileInputService>()
            .FromComponentInHierarchy()
            .AsSingle();
#else
        Container.Bind<IInputService>()
            .To<KeyboardInputService>()
            .AsSingle();
#endif

        Container.Bind<PlayerManagerSystem>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<LaserController>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<ISpawnPositionProvider>()
            .FromInstance(worldBordersController)
            .AsSingle();
    }
}