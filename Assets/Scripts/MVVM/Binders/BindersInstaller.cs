using Zenject;
using MVVM;

namespace Binders
{
    public class BindersInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            BinderFactory.RegisterBinder<TextBinder>();
        }
    }
}