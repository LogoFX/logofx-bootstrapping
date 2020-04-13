using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Solid.Bootstrapping;
using Solid.Practices.Modularity;
using TechTalk.SpecFlow;

namespace LogoFX.Bootstrapping.Tests
{
    [Binding]
    internal sealed class MiddlewareSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public MiddlewareSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"The container adapter is created")]
        public void WhenTheContainerAdapterIsCreated()
        {
            var containerAdapter = new FakeIocContainer();
            _scenarioContext.Add("containerAdapter", containerAdapter);
        }

        [When(@"The container is created")]
        public void WhenTheContainerIsCreated()
        {
            var container = new FakeContainer();
            _scenarioContext.Add("container",container);
        }

        [When(@"The bootstrapper with container adapter and composition modules is created")]
        public void WhenTheBootstrapperWithContainerAdapterAndCompositionModulesIsCreated()
        {
            var containerAdapter = _scenarioContext.Get<FakeIocContainer>("containerAdapter");
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                Registrator = containerAdapter,
                Modules = new ICompositionModule[]
                {
                    new TransientIocCompositionModule()
                }
            };
            _scenarioContext.Add("bootstrapper", bootstrapper);
        }

        [When(@"The bootstrapper with container adapter and container and composition modules is created")]
        public void WhenTheBootstrapperWithContainerAdapterAndContainerAndCompositionModulesIsCreated()
        {
            var containerAdapter = _scenarioContext.Get<FakeIocContainer>("containerAdapter");
            var container = _scenarioContext.Get<FakeContainer>("container");
            var bootstrapper = new FakeBootstrapperWithContainer
            {
                Registrator = containerAdapter,
                Container = container,
                Modules = new ICompositionModule[]
                {
                    new TransientCompositionModule()
                }
            };
            _scenarioContext.Add("bootstrapper", bootstrapper);
        }


        [When(@"The composition modules middleware is applied onto the bootstrapper with container adapter")]
        public void WhenTheCompositionModulesMiddlewareIsAppliedOntoTheBootstrapperWithContainerAdapter()
        {
            var bootstrapper = _scenarioContext.Get<FakeBootstrapperWithContainerAdapter>("bootstrapper");
            var middleware = new RegisterCompositionModulesMiddleware<FakeBootstrapperWithContainerAdapter>();
            middleware.Apply(bootstrapper);
        }

        [When(@"The composition modules middleware is applied onto the bootstrapper with container adapter and container")]
        public void WhenTheCompositionModulesMiddlewareIsAppliedOntoTheBootstrapperWithContainerAdapterAndContainer()
        {
            var bootstrapper = _scenarioContext.Get<FakeBootstrapperWithContainer>("bootstrapper");
            var middleware = new RegisterCompositionModulesMiddleware<FakeIocContainer, FakeContainer>();
            middleware.Apply(bootstrapper);
        }

        [When(@"The bootstrapper with current assembly is created")]
        public void WhenTheBootstrapperWithCurrentAssemblyIsCreated()
        {
            var containerAdapter = _scenarioContext.Get<FakeIocContainer>("containerAdapter");
            var bootstrapper = new FakeBootstrapperWithContainerAdapter
            {
                Registrator = containerAdapter,
                Assemblies = new[] { typeof(FakeIocContainer).GetTypeInfo().Assembly }
            };
            _scenarioContext.Add("bootstrapper", bootstrapper);
        }

        [When(@"The collection registration middleware is applied onto the bootstrapper")]
        public void WhenTheCollectionRegistrationMiddlewareIsAppliedOntoTheBootstrapper()
        {
            var bootstrapper = _scenarioContext.Get<FakeBootstrapperWithContainerAdapter>("bootstrapper");
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
            var registrationCollectionKey =
                _scenarioContext.ContainsKey("container") ? "container" : "containerAdapter";
            var registrationCollection = _scenarioContext.Get<IRegistrationCollection>(registrationCollectionKey);
            var registrations = registrationCollection.Registrations;
            var dependencyRegistration = registrations.First();
            return dependencyRegistration;
        }
    }
}
