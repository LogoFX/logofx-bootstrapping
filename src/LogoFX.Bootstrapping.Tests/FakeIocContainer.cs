using System;
using System.Collections.Generic;
using System.Reflection;
using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping.Tests
{
    class ContainerEntry
    {
        public ContainerEntry(
            Type interfaceType,
            Type implementationType, 
            bool isSingleton)
        {
            InterfaceType = interfaceType;
            ImplementationType = implementationType;
            IsSingleton = isSingleton;
        }

        public Type InterfaceType { get; private set; }
        public Type ImplementationType { get; private set; }
        public bool IsSingleton { get; private set; }
    }

    class InstanceEntry
    {
        public InstanceEntry(Type instanceType, object instance)
        {
            InstanceType = instanceType;
            Instance = instance;
        }

        public Type InstanceType { get; private set; }
        public object Instance { get; private set; }
    }

    class FakeIocContainer : IIocContainer, IRegistrationCollection
    {
        private readonly List<ContainerEntry> _registrations = new List<ContainerEntry>();

        private readonly List<InstanceEntry> _instances = new List<InstanceEntry>();

        IEnumerable<ContainerEntry> IRegistrationCollection.Registrations
        {
            get { return _registrations;}
        }

        internal IEnumerable<InstanceEntry> Instances
        {
            get { return _instances; }
        }

        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            _registrations.Add(new ContainerEntry(typeof (TService), typeof (TImplementation), false));
        }

        public void RegisterTransient<TService, TImplementation>(Func<TImplementation> dependencyCreator) where TImplementation : class, TService
        {
            _registrations.Add(new ContainerEntry(typeof(TService), typeof(TImplementation), false));
        }

        public void RegisterTransient<TService>() where TService : class
        {
            _registrations.Add(new ContainerEntry(typeof(TService), typeof(TService), false));
        }

        public void RegisterTransient<TService>(Func<TService> dependencyCreator) where TService : class
        {
            throw new NotImplementedException();
        }

        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            _registrations.Add(new ContainerEntry(serviceType, implementationType, false));
        }

        public void RegisterTransient(Type serviceType, Type implementationType, Func<object> dependencyCreator)
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton<TService>(Func<TService> dependencyCreator) where TService : class
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _registrations.Add(new ContainerEntry(typeof(TService), typeof(TImplementation), true));
        }

        public void RegisterSingleton<TService, TImplementation>(Func<TImplementation> dependencyCreator) where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            _registrations.Add(new ContainerEntry(serviceType, implementationType, true));
        }

        public void RegisterSingleton(Type serviceType, Type implementationType, Func<object> dependencyCreator)
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _instances.Add(new InstanceEntry(typeof(TService), instance));
        }

        public void RegisterInstance(Type dependencyType, object instance)
        {
            _instances.Add(new InstanceEntry(dependencyType, instance));
        }

        public void RegisterHandler(Type dependencyType, Func<object> handler)
        {
            throw new NotImplementedException();
        }

        public void RegisterHandler<TService>(Func<TService> handler) where TService : class
        {
            throw new NotImplementedException();
        }

        public void RegisterCollection<TService>(IEnumerable<Type> dependencyTypes) where TService : class
        {
            _registrations.Add(new ContainerEntry(typeof(IEnumerable<TService>), null, false));
        }

        public void RegisterCollection<TService>(IEnumerable<TService> dependencies) where TService : class
        {
            throw new NotImplementedException();
        }

        public void RegisterCollection(Type dependencyType, IEnumerable<Type> dependencyTypes)
        {
            _registrations.Add(new ContainerEntry(typeof(IEnumerable<>).MakeGenericType(dependencyType), null, false));
        }

        public void RegisterCollection(Type dependencyType, IEnumerable<object> dependencies)
        {
            throw new NotImplementedException();
        }

        public TService Resolve<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TService> ResolveAll<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    class FakeContainer : IRegistrationCollection
    {
        private readonly List<ContainerEntry> _registrations = new List<ContainerEntry>();

        private readonly List<InstanceEntry> _instances = new List<InstanceEntry>();

        IEnumerable<ContainerEntry> IRegistrationCollection.Registrations
        {
            get { return _registrations; }
        }

        internal IEnumerable<InstanceEntry> Instances
        {
            get { return _instances; }
        }

        public void RegisterTransient<TService, TImplementation>()
        {
            _registrations.Add(new ContainerEntry(typeof (TService), typeof (TImplementation), false));
        }
    }

    class RootObject
    {
        
    }

    interface IRegistrationCollection
    {
        IEnumerable<ContainerEntry> Registrations { get; }
    }

    class FakeBootstrapperWithContainerAdapter : IBootstrapperWithContainerAdapter<FakeIocContainer>
    {        
        public IBootstrapperWithContainerAdapter<FakeIocContainer> Use(IMiddleware<IBootstrapperWithContainerAdapter<FakeIocContainer>> middleware)
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

        public IDependencyRegistrator Registrator { get; internal set; }
        public IDependencyResolver Resolver { get; internal set; }
        public event EventHandler InitializationCompleted;
        public event EventHandler Exited;
        IBootstrapperWithRegistrator IExtensible<IBootstrapperWithRegistrator>.Use(Solid.Practices.Middleware.IMiddleware<IBootstrapperWithRegistrator> middleware)
        {
            throw new NotImplementedException();
        }        
    }

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