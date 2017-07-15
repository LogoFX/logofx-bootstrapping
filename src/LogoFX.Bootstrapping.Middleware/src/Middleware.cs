using System;
using System.Linq;
using Solid.Bootstrapping;
using Solid.Practices.Composition.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers composition modules into the ioc container adapter.
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
    /// Registers composition modules into the ioc container adapter.
    /// </summary>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
    /// <seealso cref="Solid.Practices.Middleware.IMiddleware{TBootstrapper}" />
    public class RegisterCompositionModulesMiddleware<TBootstrapper> : IMiddleware<TBootstrapper>
        where TBootstrapper : class, ICompositionModulesProvider, IHaveContainerRegistrator
    {
        /// <summary>Applies the middleware on the specified object.</summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public TBootstrapper Apply(TBootstrapper @object)
        {
            @object.Registrator.RegisterContainerCompositionModules(@object.Modules);
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
            var internalMiddleware =
                new RegisterContainerCompositionModulesMiddleware
                    <IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>, TIocContainer>();
            return internalMiddleware.Apply(@object);
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container.
    /// </summary>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>    
    public class RegisterContainerCompositionModulesMiddleware<TBootstrapper, TIocContainer> :
        IMiddleware<TBootstrapper>
        where TBootstrapper : class, IHaveContainer<TIocContainer>, ICompositionModulesProvider 
        where TIocContainer : class
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public TBootstrapper Apply(
            TBootstrapper @object)
        {
            @object.Container.RegisterContainerCompositionModules(@object.Modules);
            return @object;
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
    /// Registers collection of services. This is used in case of 
    /// loosely coupled modular application where the services are defined in separate assemblies 
    /// and/or are otherwise private.
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>    
    /// </summary>
    public class RegisterCollectionMiddleware<TBootstrapper> :
        IMiddleware<TBootstrapper> 
        where TBootstrapper : class, IHaveContainerRegistrator, IAssemblySourceProvider
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
        public TBootstrapper
            Apply(TBootstrapper @object)
        {
            RegistrationHelper.RegisterCollection(@object.Registrator, _serviceContractType,
                @object.Assemblies.Select(t => t.DefinedTypes.ToArray()).SelectMany(k => k).Select(t => t.AsType()));
            return @object;
        }
    }

    /// <summary>
    /// Registers the ioc container resolver.
    /// </summary>    
    public class RegisterResolverMiddleware : IMiddleware<IBootstrapperWithRegistrator>
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

        /// <summary>Applies the middleware on the specified object.</summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware = new RegisterResolverMiddleware<IBootstrapperWithRegistrator>(_resolver);
            return internalMiddleware.Apply(@object);
        }
    }

    /// <summary>
    /// Registers the ioc container resolver.
    /// </summary>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
    /// <seealso cref="Solid.Practices.Middleware.IMiddleware{TBootstrapper}" />
    public class RegisterResolverMiddleware<TBootstrapper> : IMiddleware<TBootstrapper>
        where TBootstrapper : class, IHaveContainerRegistrator
    {
        private readonly IIocContainerResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterResolverMiddleware{TBootstrapper}"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public RegisterResolverMiddleware(IIocContainerResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>Applies the middleware on the specified object.</summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public TBootstrapper Apply(TBootstrapper @object)
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
