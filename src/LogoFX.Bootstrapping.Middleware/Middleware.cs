using System;
using System.Linq;
using Solid.Bootstrapping;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

// ReSharper disable once CheckNamespace
namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers composition modules into the bootstrapper's registrator.
    /// </summary>    
    public class RegisterCompositionModulesMiddleware : IMiddleware<IBootstrapperWithRegistrator>        
    {
        /// <summary>Applies the middleware on the specified object.</summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware = new RegisterCompositionModulesMiddleware<IBootstrapperWithRegistrator>();
            return internalMiddleware.Apply(@object);
        }
    }        

    /// <summary>
    /// Registers composition modules into the bootstrapper's ioc container.
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
            var internalMiddleware =
                new RegisterContainerCompositionModulesMiddleware
                    <IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>, TIocContainer>();
            return internalMiddleware.Apply(@object);
        }
    }    

    /// <summary>
    /// Registers the collection of <see cref="IBootstrapperCompositionModule"/> modules.
    /// </summary>
    /// <seealso cref="Solid.Practices.Middleware.IMiddleware{IBootstrapperWithRegistrator}" />
    public class RegisterBootstrapperCompositionModulesMiddleware : IMiddleware<IBootstrapperWithRegistrator>
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public IBootstrapperWithRegistrator Apply(IBootstrapperWithRegistrator @object)
        {
            foreach (var module in @object.Modules.OfType<IBootstrapperCompositionModule>())
            {
                module.RegisterModule(@object);
            }
            return @object;
        }
    }

    /// <summary>
    /// Registers collection of services. This is used in case of 
    /// loosely coupled modular application where the services are defined in separate assemblies 
    /// and/or are otherwise private.
    /// </summary>
    public class RegisterCollectionMiddleware :
        IMiddleware<IBootstrapperWithRegistrator>
    {
        private readonly Type _serviceContractType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCollectionMiddleware"/> class.
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
        public IBootstrapperWithRegistrator
            Apply(IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware =
                new RegisterCollectionMiddleware<IBootstrapperWithRegistrator>(_serviceContractType);
            return internalMiddleware.Apply(@object);
        }
    }        

    /// <summary>
    /// Registers the dependency resolver.
    /// </summary>    
    public class RegisterResolverMiddleware : IMiddleware<IBootstrapperWithRegistrator>
    {
        private readonly IDependencyResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterResolverMiddleware"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public RegisterResolverMiddleware(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>Applies the middleware on the specified object.</summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware = new RegisterResolverMiddleware<IBootstrapperWithRegistrator>(_resolver);
            return internalMiddleware.Apply(@object);
        }
    }        
}
