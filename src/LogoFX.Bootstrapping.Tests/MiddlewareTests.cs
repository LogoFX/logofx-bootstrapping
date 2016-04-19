using System.Linq;
using Shouldly;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;
using Xunit;

namespace LogoFX.Bootstrapping.Tests
{
    public class MiddlewareTests
    {
        [Fact]
        public void WhenRegisterCoreMiddlewareIsApplied_ThenCoreElementsAreRegistered()
        {
            var container = new FakeIocContainer();
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                ContainerAdapter = container
            };
            
            var middleware = new RegisterContainerMiddleware<FakeIocContainer>();
            middleware.Apply(bootstrapper);
            
            var instances = container.Instances;
            var actualTypeContainerRegistration = instances.First();
            actualTypeContainerRegistration.Instance.ShouldBe(container);
            actualTypeContainerRegistration.InstanceType.ShouldBe(typeof(FakeIocContainer));
            var interfaceTypeContainerRegistration = instances.Last();
            interfaceTypeContainerRegistration.Instance.ShouldBe(container);
            interfaceTypeContainerRegistration.InstanceType.ShouldBe(typeof(IIocContainer));
        }

        [Fact]
        public void
            GivenCompositionModuleRegistersDependencyAsSingleton_WhenRegisterContainerAdapterCompositionModulesMiddlewareIsApplied_ThenDependencyIsRegistered
            ()
        {
            var container = new FakeIocContainer();
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                ContainerAdapter = container,
                Modules = new ICompositionModule[]
                {
                    new TransientIocCompositionModule()
                }
            };

            var middleware = new RegisterCompositionModulesMiddleware<FakeIocContainer>();
            middleware.Apply(bootstrapper);

            var registrations = container.Registrations;
            var dependencyRegistration = registrations.First();
            dependencyRegistration.ImplementationType.ShouldBe(typeof(TransientDependency));
            dependencyRegistration.InterfaceType.ShouldBe(typeof(IDependency));
            dependencyRegistration.IsSingleton.ShouldBe(false);
        }

        [Fact]
        public void
            GivenConcreteCompositionModuleRegistersDependencyAsSingleton_WhenRegisterContainerCompositionModulesMiddlewareIsApplied_ThenDependencyIsRegistered
            ()
        {
            var containerAdapter = new FakeIocContainer();
            var container = new FakeContainer();
            var bootstrapper = new FakeBootstrapperWithContainer
            {
                ContainerAdapter = containerAdapter,
                Container = container,
                Modules = new ICompositionModule[]
                {
                    new TransientCompositionModule()
                }
            };

            var middleware = new RegisterCompositionModulesMiddleware<FakeIocContainer, FakeContainer>();
            middleware.Apply(bootstrapper);

            var registrations = container.Registrations;
            var dependencyRegistration = registrations.First();
            dependencyRegistration.ImplementationType.ShouldBe(typeof(TransientDependency));
            dependencyRegistration.InterfaceType.ShouldBe(typeof(IDependency));
            dependencyRegistration.IsSingleton.ShouldBe(false);
        }
    }
}
