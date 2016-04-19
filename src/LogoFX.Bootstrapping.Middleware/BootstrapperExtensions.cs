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
        /// Uses the module root objects registration middleware.
        /// </summary>        
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>                
        /// <param name="bootstrapper">The bootstrapper.</param>        
        /// <param name="moduleRootObjectType">The type of module root object.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TIocContainerAdapter>
            UseModuleRootObjectsRegistration<TIocContainerAdapter>(
            this IBootstrapperWithContainerAdapter<TIocContainerAdapter> bootstrapper,            
            Type moduleRootObjectType)
            where TIocContainerAdapter : IIocContainer
        {
            bootstrapper.Use(
                new RegisterModuleRootObjectsMiddleware
                    <TIocContainerAdapter>(moduleRootObjectType));
            return bootstrapper;
        }
    }
}