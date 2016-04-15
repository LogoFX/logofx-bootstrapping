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
        /// <typeparam name="TModuleRootObject">The type of the module root object.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>
            UseModuleRootObjectsRegistration<TRootObject, TIocContainerAdapter, TModuleRootObject>(
            this IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> bootstrapper)
            where TIocContainerAdapter : IIocContainer
        {
            bootstrapper.Use(
                new RegisterModuleRootObjectsMiddleware
                    <TRootObject, TIocContainerAdapter, TModuleRootObject>());
            return bootstrapper;
        }
    }
}