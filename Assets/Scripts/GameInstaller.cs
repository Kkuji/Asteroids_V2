using System.Collections;
using System.Collections.Generic;
using Managment;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform playerShip;
    [SerializeField] private Factory factory;

    public override void InstallBindings()
    {
        Container.Bind<ConfigService>().AsSingle();
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

        Container.Bind<UIInfo>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}