using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Solid.Bootstrapping;
using Solid.Practices.Modularity;
using TechTalk.SpecFlow;

namespace LogoFX.Bootstrapping.Specs
{
    [Binding]
    internal sealed class MiddlewareSteps
    {
        private readonly MiddlewareScenarioDataStore _scenarioDataStore;

        public MiddlewareSteps(ScenarioContext scenarioContext)
        {
            _scenarioDataStore = new MiddlewareScenarioDataStore(scenarioContext);
        }

        [When(@"The container adapter is created")]
        public void WhenTheContainerAdapterIsCreated()
        {
            var containerAdapter = new FakeIocContainer();
            _scenarioDataStore.ContainerAdapter = containerAdapter;
        }

        [When(@"The container is created")]
        public void WhenTheContainerIsCreated()
        {
            var container = new FakeContainer();
            _scenarioDataStore.Container = container;
        }

        [When(@"The bootstrapper with container adapter and composition modules is created")]
        public void WhenTheBootstrapperWithContainerAdapterAndCompositionModulesIsCreated()
        {
            var containerAdapter = _scenarioDataStore.ContainerAdapter;
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                Registrator = containerAdapter,
                Modules = new ICompositionModule[]
                {
                    new TransientIocCompositionModule()
                }
            };
            _scenarioDataStore.Bootstrapper = bootstrapper;
        }

        [When(@"The bootstrapper with container adapter and container and composition modules is created")]
        public void WhenTheBootstrapperWithContainerAdapterAndContainerAndCompositionModulesIsCreated()
        {
            var containerAdapter = _scenarioDataStore.ContainerAdapter;
            var container = _scenarioDataStore.Container;
            var bootstrapper = new FakeBootstrapperWithContainer
            {
                Registrator = containerAdapter,
                Container = container,
                Modules = new ICompositionModule[]
                {
                    new TransientCompositionModule()
                }
            };
            _scenarioDataStore.Bootstrapper = bootstrapper;
        }

        [When(@"The composition modules middleware is applied onto the bootstrapper with container adapter")]
        public void WhenTheCompositionModulesMiddlewareIsAppliedOntoTheBootstrapperWithContainerAdapter()
        {
            var bootstrapper = _scenarioDataStore.Bootstrapper as FakeBootstrapperWithContainerAdapter;
            var middleware = new RegisterCompositionModulesMiddleware<FakeBootstrapperWithContainerAdapter>();
            middleware.Apply(bootstrapper);
        }

        [When(@"The composition modules middleware is applied onto the bootstrapper with container adapter and container")]
        public void WhenTheCompositionModulesMiddlewareIsAppliedOntoTheBootstrapperWithContainerAdapterAndContainer()
        {
            var bootstrapper = _scenarioDataStore.Bootstrapper as FakeBootstrapperWithContainer;
            var middleware = new RegisterCompositionModulesMiddleware<FakeIocContainer, FakeContainer>();
            middleware.Apply(bootstrapper);
        }

        [When(@"The bootstrapper with current assembly is created")]
        public void WhenTheBootstrapperWithCurrentAssemblyIsCreated()
        {
            var containerAdapter = _scenarioDataStore.ContainerAdapter;
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                Registrator = containerAdapter,
                Assemblies = new[] { typeof(FakeIocContainer).GetTypeInfo().Assembly }
            };
            _scenarioDataStore.Bootstrapper = bootstrapper;
        }

        [When(@"The collection registration middleware is applied onto the bootstrapper")]
        public void WhenTheCollectionRegistrationMiddlewareIsAppliedOntoTheBootstrapper()
        {
            var bootstrapper = _scenarioDataStore.Bootstrapper as FakeBootstrapperWithContainerAdapter;
            var middleware = new RegisterCollectionMiddleware<FakeBootstrapperWithContainerAdapter>(typeof(IServiceContract));
            middleware.Apply(bootstrapper);
        }

        [Then(@"The registered dependency should be of correct type")]
        public void ThenTheRegisteredDependencyShouldBeOfCorrectType()
        {
            var dependencyRegistration = GetDependencyRegistration();
            dependencyRegistration.ImplementationType.Should().Be(typeof(TransientDependency));
            dependencyRegistration.InterfaceType.Should().Be(typeof(IDependency));
        }

        [Then(@"The registered dependency should be transient")]
        public void ThenTheRegisteredDependencyShouldBeTransient()
        {
            var dependencyRegistration = GetDependencyRegistration();
            dependencyRegistration.IsSingleton.Should().Be(false);
        }

        [Then(@"The dependencies are registered as a collection")]
        public void ThenTheDependenciesAreRegisteredAsACollection()
        {
            var dependencyRegistration = GetDependencyRegistration();
            (dependencyRegistration.InterfaceType == typeof(IEnumerable<IServiceContract>)).Should().BeTrue();
        }

        private ContainerEntry GetDependencyRegistration()
        {
            var registrationCollection = _scenarioDataStore.Container != null
                ? (IRegistrationCollection) _scenarioDataStore.Container
                : _scenarioDataStore.ContainerAdapter;
            var registrations = registrationCollection.Registrations;
            var dependencyRegistration = registrations.First();
            return dependencyRegistration;
        }
    }
}
