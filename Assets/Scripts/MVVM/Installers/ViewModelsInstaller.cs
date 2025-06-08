using System.ComponentModel;
using Unity.VisualScripting;
using Zenject;

namespace UI.ViewModels
{
    public class ViewModelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<PositionViewModel>()
                .AsSingle()
                .NonLazy();

            this.Container.BindInterfacesAndSelfTo<AngleViewModel>()
                .AsSingle()
                .NonLazy();

            this.Container.BindInterfacesAndSelfTo<SpeedViewModel>()
                .AsSingle()
                .NonLazy();

            this.Container.BindInterfacesAndSelfTo<LaserCooldownViewModel>()
                .AsSingle()
                .NonLazy();

            this.Container.BindInterfacesAndSelfTo<PointsViewModel>()
                .AsSingle()
                .NonLazy();

            this.Container.BindInterfacesAndSelfTo<HealthViewModel>()
                .AsSingle()
                .NonLazy();

            this.Container.BindInterfacesAndSelfTo<LaserAvailableShotsViewModel>()
                .AsSingle()
                .NonLazy();
        }
    }
}