using System.Collections.Generic;
using System.Linq;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// The ioc container registrator extension methods.
    /// </summary>
    public static class ContainerRegistratorExtensions
    {        
        /// <summary>
        /// Registers the composition modules into the ioc container registrator.
        /// </summary>
        /// <typeparam name="TIocContainer">The type of the ioc container registrator.</typeparam>
        /// <param name="iocContainer">The ioc container registrator.</param>
        /// <param name="compositionModules">The composition modules.</param>
        public static void RegisterContainerAdapterCompositionModules<TIocContainer>(
            this TIocContainer iocContainer,
            IEnumerable<ICompositionModule> compositionModules)
            where TIocContainer : class, IIocContainerRegistrator
        {
            var modules = compositionModules as ICompositionModule[] ?? compositionModules.ToArray();
            var middlewares = new List<IMiddleware<TIocContainer>>(new IMiddleware<TIocContainer>[]
            {
                new ContainerRegistrationMiddleware<TIocContainer, IIocContainerRegistrator>(modules),
                new ContainerPlainRegistrationMiddleware<TIocContainer>(modules),
                new ContainerHierarchicalRegistrationMiddleware<TIocContainer>(modules)
            });
            MiddlewareApplier.ApplyMiddlewares(iocContainer, middlewares);
        }
    }
}
