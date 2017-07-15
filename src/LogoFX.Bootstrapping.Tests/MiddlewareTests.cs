using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Solid.Practices.Modularity;
using Xunit;

namespace LogoFX.Bootstrapping.Tests
{
    public class MiddlewareTests
    {        
        [Fact]
        public void
            GivenCompositionModuleRegistersDependencyAsSingleton_WhenRegisterContainerAdapterCompositionModulesMiddlewareIsApplied_ThenDependencyIsRegistered
            ()
        {
            var container = new FakeIocContainer();
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                Registrator = container,
                Modules = new ICompositionModule[]
                {
                    new TransientIocCompositionModule()
                }
            };

            var middleware = new RegisterCompositionModulesMiddleware<FakeBootstrapperWithContainerAdapter>();
            middleware.Apply(bootstrapper);

            var registrations = container.Registrations;
            var dependencyRegistration = registrations.First();
            dependencyRegistration.ImplementationType.Should().Be(typeof(TransientDependency));
            dependencyRegistration.InterfaceType.Should().Be(typeof(IDependency));
            dependencyRegistration.IsSingleton.Should().Be(false);
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
                Registrator = containerAdapter,
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
            dependencyRegistration.ImplementationType.Should().Be(typeof(TransientDependency));
            dependencyRegistration.InterfaceType.Should().Be(typeof(IDependency));
            dependencyRegistration.IsSingleton.Should().Be(false);
        }

        [Fact]
        public void
            GivenThereAreTwoServicesThatImplementTheContract_WhenRegisterCollectionMiddlewareIsApplied_ThenBothServicesAreRegistered
            ()
        {
            var containerAdapter = new FakeIocContainer();            
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                Registrator = containerAdapter,                
                Assemblies = new[] { typeof(FakeIocContainer).GetTypeInfo().Assembly }
            };

            var middleware = new RegisterCollectionMiddleware(typeof (IServiceContract));
            middleware.Apply(bootstrapper);

            var registrations = containerAdapter.Registrations;
            var dependencyRegistration = registrations.First();
            (dependencyRegistration.InterfaceType == typeof(IEnumerable<IServiceContract>)).Should().BeTrue();
        }
    }
}
