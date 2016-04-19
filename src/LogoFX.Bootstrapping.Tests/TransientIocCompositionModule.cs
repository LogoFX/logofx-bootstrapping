using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping.Tests
{
    class TransientIocCompositionModule : ICompositionModule<IIocContainer>
    {
        public void RegisterModule(IIocContainer iocContainer)
        {
            iocContainer.RegisterTransient<IDependency, TransientDependency>();
        }
    }
}