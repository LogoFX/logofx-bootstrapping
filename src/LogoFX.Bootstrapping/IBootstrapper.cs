using System.Collections.Generic;
using System.Reflection;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Represents the bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Gets the composition modules.
        /// </summary>
        /// <value>
        /// The composition modules.
        /// </value>
        IEnumerable<ICompositionModule> Modules { get; }

        /// <summary>
        /// Gets the assemblies which can be inspected for the additional components.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        Assembly[] Assemblies { get; }

        /// <summary>
        /// Starts the bootstrapping.
        /// </summary>
        void Initialize();        

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        IBootstrapper Use(
            IMiddleware<IBootstrapper> middleware);
    }

    /// <summary>
    /// Represents bootstrapper with ioc container adapter.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <seealso cref="IBootstrapper" />
    public interface IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> : IBootstrapper
    {
        /// <summary>
        /// Gets the container adapter.
        /// </summary>
        /// <value>
        /// The container adapter.
        /// </value>
        TIocContainerAdapter ContainerAdapter { get; }

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> Use(
            IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>> middleware);
    }

    /// <summary>
    /// Represents bootstrapper with ioc container and ioc container adapter.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>
    /// <seealso cref="IBootstrapperWithContainerAdapter{TRootObject, TIocContainerAdapter}" />
    public interface IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer> : 
        IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        TIocContainer Container { get; }

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer>> middleware);
    }    
}
