using System;
using Solid.Bootstrapping;
using Solid.Practices.IoC;

// ReSharper disable once CheckNamespace
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
    }
}