using System;
using Solid.Practices.IoC;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Bootstrapper extension methods.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the module root objects registration middleware.
        /// </summary>
        /// <typeparam name="TRootObject">The type of the root object.</typeparam>
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>                
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="moduleRootObjectType">The type of module root object.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>
            UseModuleRootObjectsRegistration<TRootObject, TIocContainerAdapter>(
            this IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> bootstrapper,
            Type moduleRootObjectType)
            where TIocContainerAdapter : IIocContainer
        {
            bootstrapper.Use(
                new RegisterModuleRootObjectsMiddleware
                    <TRootObject, TIocContainerAdapter>(moduleRootObjectType));
            return bootstrapper;
        }
    }
}