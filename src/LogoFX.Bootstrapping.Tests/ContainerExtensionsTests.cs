using System.Linq;
using Shouldly;
using Solid.Practices.Modularity;
using Xunit;

namespace LogoFX.Bootstrapping.Tests
{
    public class ContainerExtensionsTests
    {
        [Fact]
        public void
            GivenCompositionModuleRegistersDependencyAsSingleton_WhenCompositionModulesRegistrationIsInvoked_ThenDependencyIsRegistered
            ()
        {
            var container = new FakeContainer();
            container.RegisterContainerCompositionModules(new ICompositionModule[]
            {
                new TransientCompositionModule()
            });

            var registrations = container.Registrations;
            var dependencyRegistration = registrations.First();
            dependencyRegistration.ImplementationType.ShouldBe(typeof(TransientDependency));
            dependencyRegistration.InterfaceType.ShouldBe(typeof(IDependency));
            dependencyRegistration.IsSingleton.ShouldBe(false);
        }
    }
}