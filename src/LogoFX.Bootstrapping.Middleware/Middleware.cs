using System;
using System.Linq;
using System.Reflection;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers the ioc container adapter.
    /// </summary>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterContainerMiddleware<TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>       
        where TIocContainerAdapter : class, IIocContainer
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TIocContainerAdapter> @object)
        {
            @object.ContainerAdapter.RegisterContainer();
            return @object;
        }
    }
    
    /// <summary>
    /// Registers composition modules into the ioc container adapter.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>        
        where TIocContainerAdapter : class, IIocContainer
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TIocContainerAdapter> @object)
        {
            @object.ContainerAdapter.RegisterContainerAdapterCompositionModules(@object.Modules);
            return @object;
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TIocContainerAdapter, TIocContainer> :
        IMiddleware<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>>        
        where TIocContainerAdapter : class, IIocContainer
        where TIocContainer : class
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> Apply(
            IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> @object)
        {
            @object.Container.RegisterContainerCompositionModules(@object.Modules);
            return @object;
        }
    }

    /// <summary>
    /// Registers the root objects of the modules. This is used in case of 
    /// loosely coupled module-oriented application where the modules have their own dependencies 
    /// that need to be injected during their creation.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    public class RegisterModuleRootObjectsMiddleware<TIocContainerAdapter> : 
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>
        where TIocContainerAdapter : IIocContainer
    {        
        private readonly IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> _innerMiddleware;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModuleRootObjectsMiddleware{TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="moduleRootObjectType">The type of the module root object.</param>
        public RegisterModuleRootObjectsMiddleware(Type moduleRootObjectType)
        {     
            _innerMiddleware = new RegisterCollectionMiddleware<TIocContainerAdapter>(moduleRootObjectType);
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter>
            Apply(IBootstrapperWithContainerAdapter<TIocContainerAdapter> @object)
        {
            return _innerMiddleware.Apply(@object);
        }
    }

    /// <summary>
    /// Registers collection of services. This is used in case of 
    /// loosely coupled module-oriented application where the services are defined in separate assemblies 
    /// and/or are otherwise invisible.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    public class RegisterCollectionMiddleware<TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>
        where TIocContainerAdapter : IIocContainer
    {
        private readonly Type _serviceContractType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModuleRootObjectsMiddleware{TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="serviceContractType">The type of the module root object.</param>
        public RegisterCollectionMiddleware(Type serviceContractType)
        {
            _serviceContractType = serviceContractType;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter>
            Apply(IBootstrapperWithContainerAdapter<TIocContainerAdapter> @object)
        {
            var typeInfo = _serviceContractType.GetTypeInfo();
            var serviceTypes = @object.Assemblies.Select(t => t.DefinedTypes.ToArray()).SelectMany(k => k).Where(t =>
                t.IsInterface == false && t.IsAbstract == false &&
                typeInfo.IsAssignableFrom(t)).Select(t => t.AsType());
            @object.ContainerAdapter.RegisterCollection(_serviceContractType, serviceTypes);
            return @object;
        }
    }
}
