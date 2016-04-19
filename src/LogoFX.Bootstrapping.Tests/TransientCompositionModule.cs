using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping.Tests
{
    class TransientCompositionModule : ICompositionModule<FakeContainer>
    {
        public void RegisterModule(FakeContainer iocContainer)
        {
            iocContainer.RegisterTransient<IDependency, TransientDependency>();
        }
    }
}