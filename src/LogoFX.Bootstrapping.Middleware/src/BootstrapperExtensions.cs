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
        /// Applies the collection registration middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="serviceContractType">The type of the service contract.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerRegistrator
            ApplyCollectionRegistration(
            this IBootstrapperWithContainerRegistrator bootstrapper,
            Type serviceContractType)
        {
            var middleware = new RegisterCollectionMiddleware(serviceContractType);
            middleware.Apply(bootstrapper);
            return bootstrapper;
        }

        /// <summary>
        /// Applies the collection registration middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <typeparam name="TService">The type of the service contract.</typeparam>
        /// <returns></returns>
        public static IBootstrapperWithContainerRegistrator
            ApplyCollectionRegistration<TService>(
            this IBootstrapperWithContainerRegistrator bootstrapper)
        {
           return ApplyCollectionRegistration(bootstrapper, typeof(TService));
        }

        /// <summary>
        /// Uses the collection registration middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="serviceContractType">The type of the service contract.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerRegistrator
            UseCollectionRegistration(
            this IBootstrapperWithContainerRegistrator bootstrapper,
            Type serviceContractType)
        {
            bootstrapper.Use(
                new RegisterCollectionMiddleware(serviceContractType));
            return bootstrapper;
        }

        /// <summary>
        /// Uses the resolver middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerRegistrator
            UseResolver(
            this IBootstrapperWithContainerRegistrator bootstrapper,
            IIocContainerResolver resolver)
        {
            bootstrapper.Use(new RegisterResolverMiddleware<IBootstrapperWithContainerRegistrator>(resolver));
            return bootstrapper;            
        }

        /// <summary>
        /// Uses the bootstrapper composition middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerRegistrator UseBootstrapperComposition(
           this IBootstrapperWithContainerRegistrator bootstrapper)
        {
            bootstrapper.Use(new RegisterBootstrapperCompositionModulesMiddleware());
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