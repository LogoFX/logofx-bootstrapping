using System;
using System.Collections.Generic;
using System.Linq;
using Solid.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// The bootstrapper extension methods.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the collection registration middleware.
        /// </summary>
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="serviceContractType">The type of the service contract.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TIocContainerAdapter>
            UseCollectionRegistration<TIocContainerAdapter>(
            this IBootstrapperWithContainerAdapter<TIocContainerAdapter> bootstrapper,
            Type serviceContractType)
            where TIocContainerAdapter : IIocContainer
        {
            bootstrapper.Use(
                new RegisterCollectionMiddleware<TIocContainerAdapter>(serviceContractType));
            return bootstrapper;
        }

        /// <summary>
        /// Uses the resolver middleware.
        /// </summary>
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TIocContainerAdapter>
            UseResolver<TIocContainerAdapter>(
            this IBootstrapperWithContainerAdapter<TIocContainerAdapter> bootstrapper,
            IIocContainerResolver resolver) 
            where TIocContainerAdapter : class, IIocContainer
        {
            bootstrapper.Use(new RegisterResolverMiddleware<TIocContainerAdapter>(resolver));
            return bootstrapper;            
        }

        /// <summary>
        /// Extends the bootstrapper's functionality by using the specified collection
        /// of ioc container registrator middlewares.
        /// </summary>
        /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="middlewares">The middlewares.</param>
        /// <returns></returns>
        public static TBootstrapper UseMany<TBootstrapper>(
            this TBootstrapper bootstrapper,
            IEnumerable<IMiddleware<IIocContainerRegistrator>> middlewares) 
            where TBootstrapper : class, IHaveContainerRegistrator, IExtensible<TBootstrapper>
        {
            var bootstrapperMiddlewares =
                middlewares.Select(
                    t =>
                        new UseContainerRegistratorMiddleware<TBootstrapper>(t));
            foreach (var bootstrapperMiddleware in bootstrapperMiddlewares)
            {
                bootstrapper.Use(bootstrapperMiddleware);
            }
            return bootstrapper;
        }
    }
}