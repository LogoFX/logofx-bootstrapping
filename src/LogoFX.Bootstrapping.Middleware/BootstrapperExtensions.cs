using System;
using Solid.Practices.IoC;

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
    }
}