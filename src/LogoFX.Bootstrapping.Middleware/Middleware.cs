using System;
using System.Linq;
using Solid.Bootstrapping;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers composition modules into the ioc container adapter.
    /// </summary>
    public class RegisterCompositionModulesMiddleware :
        IMiddleware<IBootstrapperWithContainerRegistrator>
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerRegistrator Apply(
            IBootstrapperWithContainerRegistrator @object)
        {
            @object.Registrator.RegisterContainerAdapterCompositionModules(@object.Modules);
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
    /// Registers collection of services. This is used in case of 
    /// loosely coupled modular application where the services are defined in separate assemblies 
    /// and/or are otherwise private.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    public class RegisterCollectionMiddleware<TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>
        where TIocContainerAdapter : IIocContainer
    {
        private readonly Type _serviceContractType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCollectionMiddleware{TIocContainerAdapter}"/> class.
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
            RegistrationHelper.RegisterCollection(@object.Registrator, _serviceContractType,
                @object.Assemblies.Select(t => t.DefinedTypes.ToArray()).SelectMany(k => k).Select(t => t.AsType()));         
            return @object;
        }
    }

    /// <summary>
    /// Registers the ioc container resolver.
    /// </summary>
    public class RegisterResolverMiddleware :
        IMiddleware<IBootstrapperWithContainerRegistrator>
    {
        private readonly IIocContainerResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterResolverMiddleware"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public RegisterResolverMiddleware(IIocContainerResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerRegistrator Apply(
            IBootstrapperWithContainerRegistrator @object)
        {
            @object.Registrator.RegisterInstance(_resolver);
            return @object;
        }
    }

    /// <summary>
    /// Extends the bootstrapper's functionality by using the 
    /// specified ioc container registrator middleware.
    /// </summary>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
    /// <seealso cref="Solid.Practices.Middleware.IMiddleware{TBootstrapper}" />
    public class UseContainerRegistratorMiddleware<TBootstrapper> : IMiddleware<TBootstrapper>
        where TBootstrapper : class, IHaveContainerRegistrator
    {
        private readonly IMiddleware<IIocContainerRegistrator> _middleware;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseContainerRegistratorMiddleware{TBootstrapper}"/> class.
        /// </summary>
        /// <param name="middleware">The ioc container registrator middleware.</param>
        public UseContainerRegistratorMiddleware(IMiddleware<IIocContainerRegistrator> middleware)
        {
            _middleware = middleware;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public TBootstrapper Apply(
            TBootstrapper @object)
        {
            _middleware.Apply(@object.Registrator);
            return @object;
        }
    }
}
