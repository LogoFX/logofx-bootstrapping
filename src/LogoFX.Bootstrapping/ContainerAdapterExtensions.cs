using System.Collections.Generic;
using System.Linq;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// The ioc container adapter extension methods.
    /// </summary>
    public static class ContainerAdapterExtensions
    {
        /// <summary>
        /// Registers the ioc container adapter and root object.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        public static void RegisterCore<TRootObject, TIocContainerAdapter>(
            this TIocContainerAdapter iocContainerAdapter)
            where TIocContainerAdapter : class, IIocContainer 
            where TRootObject : class
        {
            iocContainerAdapter.RegisterSingleton<TRootObject, TRootObject>();
            iocContainerAdapter.RegisterInstance(iocContainerAdapter);
            iocContainerAdapter.RegisterInstance<IIocContainer>(iocContainerAdapter);
        }

        /// <summary>
        /// Registers the composition modules into the ioc container adapter.
        /// </summary>
        /// <typeparam name="TIocContainer">The type of the ioc container adapter.</typeparam>
        /// <param name="iocContainer">The ioc container adapter.</param>
        /// <param name="compositionModules">The composition modules.</param>
        public static void RegisterContainerAdapterCompositionModules<TIocContainer>(
            this TIocContainer iocContainer,
            IEnumerable<ICompositionModule> compositionModules)
            where TIocContainer : class, IIocContainer
        {
            var modules = compositionModules as ICompositionModule[] ?? compositionModules.ToArray();
            var middlewares = new List<IMiddleware<TIocContainer>>(new IMiddleware<TIocContainer>[]
            {
                new ContainerRegistrationMiddleware<TIocContainer, IIocContainer>(modules),
                new ContainerPlainRegistrationMiddleware<TIocContainer>(modules),
                new ContainerHierarchicalRegistrationMiddleware<TIocContainer>(modules)
            });

            MiddlewareApplier.ApplyMiddlewares(iocContainer, middlewares);
        }
    }
}
