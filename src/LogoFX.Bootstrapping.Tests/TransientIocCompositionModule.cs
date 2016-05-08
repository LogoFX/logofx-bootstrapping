using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping.Tests
{
    class TransientIocCompositionModule : ICompositionModule<IIocContainerRegistrator>
    {
        public void RegisterModule(IIocContainerRegistrator iocContainerRegistrator)
        {
            iocContainerRegistrator.RegisterTransient<IDependency, TransientDependency>();
        }
    }
}