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
        public static IBootstrapperWithRegistrator
            ApplyCollectionRegistration(
            this IBootstrapperWithRegistrator bootstrapper,
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
        public static IBootstrapperWithRegistrator
            ApplyCollectionRegistration<TService>(
            this IBootstrapperWithRegistrator bootstrapper)
        {
           return ApplyCollectionRegistration(bootstrapper, typeof(TService));
        }

        /// <summary>
        /// Uses the collection registration middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="serviceContractType">The type of the service contract.</param>
        /// <returns></returns>
        public static IBootstrapperWithRegistrator
            UseCollectionRegistration(
            this IBootstrapperWithRegistrator bootstrapper,
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
        public static IBootstrapperWithRegistrator
            UseResolver(
            this IBootstrapperWithRegistrator bootstrapper,
            IDependencyResolver resolver)
        {
            bootstrapper.Use(new RegisterResolverMiddleware<IBootstrapperWithRegistrator>(resolver));
            return bootstrapper;            
        }

        /// <summary>
        /// Uses the bootstrapper composition middleware.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns></returns>
        public static IBootstrapperWithRegistrator UseBootstrapperComposition(
           this IBootstrapperWithRegistrator bootstrapper)
        {
            bootstrapper.Use(new RegisterBootstrapperCompositionModulesMiddleware());
            return bootstrapper;
        }

        /// <summary>
        /// Extends the bootstrapper's functionality by using the specified collection
        /// of dependency registrator middlewares.
        /// </summary>
        /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="middlewares">The middlewares.</param>
        /// <returns></returns>
        public static TBootstrapper UseMany<TBootstrapper>(
            this TBootstrapper bootstrapper,
            IEnumerable<IMiddleware<IDependencyRegistrator>> middlewares) 
            where TBootstrapper : class, IHaveRegistrator, IExtensible<TBootstrapper>
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