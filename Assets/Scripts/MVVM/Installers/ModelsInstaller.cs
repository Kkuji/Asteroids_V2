using Gameplay;
using Zenject;

namespace MVVM.Installers
{
    public class ModelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container
                .Bind<PositionStorage>()
                .AsSingle()
                .NonLazy();

            this.Container
                .Bind<AngleStorage>()
                .AsSingle()
                .NonLazy();

            this.Container
                .Bind<SpeedStorage>()
                .AsSingle()
                .NonLazy();

            this.Container
                .Bind<LaserCooldownStorage>()
                .AsSingle()
                .NonLazy();

            this.Container
                .Bind<PointsStorage>()
                .AsSingle()
                .NonLazy();

            this.Container
                .Bind<HealthStorage>()
                .AsSingle()
                .NonLazy();

            this.Container
                .Bind<LaserAvailableShotsStorage>()
                .AsSingle()
                .NonLazy();
        }
    }
}