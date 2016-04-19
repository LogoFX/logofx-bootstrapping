using System.Linq;
using Shouldly;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;
using Xunit;

namespace LogoFX.Bootstrapping.Tests
{
    public class ContainerAdapterExtensionsTests
    {
        [Fact]
        public void WhenCoreRegistrationIsInvoked_ThenCoreElementsAreRegistered()
        {
            var container = new FakeIocContainer();
            container.RegisterContainer();
            
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
            GivenCompositionModuleRegistersDependencyAsSingleton_WhenCompositionModulesRegistrationIsInvoked_ThenDependencyIsRegistered
            ()
        {            
            var container = new FakeIocContainer();
            container.RegisterContainerAdapterCompositionModules(new ICompositionModule[]
            {
                new TransientIocCompositionModule()
            });

            var registrations = container.Registrations;
            var dependencyRegistration = registrations.First();
            dependencyRegistration.ImplementationType.ShouldBe(typeof(TransientDependency));
            dependencyRegistration.InterfaceType.ShouldBe(typeof(IDependency));
            dependencyRegistration.IsSingleton.ShouldBe(false);
        }
    }
}
