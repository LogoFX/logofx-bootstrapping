using System;
using System.Collections.Generic;
using System.Reflection;
using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping.Specs
{
    class FakeBootstrapperWithContainer : IBootstrapperWithContainer<FakeIocContainer, FakeContainer>
    {
        public IBootstrapperWithContainerAdapter<FakeIocContainer> Use(IMiddleware<IBootstrapperWithContainerAdapter<FakeIocContainer>> middleware)
        {
            throw new NotImplementedException();
        }

        public IBootstrapperWithContainer<FakeIocContainer, FakeContainer> Use(IMiddleware<IBootstrapperWithContainer<FakeIocContainer, FakeContainer>> middleware)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICompositionModule> Modules { get; internal set; }
        public IEnumerable<Assembly> Assemblies { get; internal set; }
        public IEnumerable<Exception> Errors { get; internal set; }

        public IBootstrapper Use(IMiddleware<IBootstrapper> middleware)
        {
            throw new NotImplementedException();
        }

        IBootstrapperWithRegistrator IExtensible<IBootstrapperWithRegistrator>.Use(Solid.Practices.Middleware.IMiddleware<IBootstrapperWithRegistrator> middleware)
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistrator Registrator { get; internal set; }
        public IDependencyResolver Resolver { get; internal set; }
        public FakeContainer Container { get; internal set; }
        public event EventHandler InitializationCompleted;
        public event EventHandler Exited;
    }
}