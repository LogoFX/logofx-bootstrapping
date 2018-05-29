using System;
using System.Collections.Generic;
using Solid.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.Composition.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Exposes additional initialization information.
    /// </summary>
    public interface IInitializationInfo
    {
        /// <summary>
        /// Occurs when initialization is completed and the application starts.
        /// </summary>
        event EventHandler InitializationCompleted;
    }

    /// <summary>
    /// Gets the collection of errors that happen during initialization.
    /// </summary>
    public interface IHaveErrors
    {
        /// <summary>
        /// The errors.
        /// </summary>
        IEnumerable<Exception> Errors { get; }
    }

    /// <summary>
    /// Exposes additional shutdown information.
    /// </summary>
    public interface IExitInfo
    {
        /// <summary>
        /// Occurs when the application shuts down.
        /// </summary>
        event EventHandler Exited;
    }

    /// <summary>
    /// Represents the bootstrapper.
    /// </summary>
    public interface IBootstrapper : IInitializable, IInitializationInfo, IExitInfo, ICompositionModulesProvider,
        IExtensible<IBootstrapper>, IAssemblySourceProvider, IHaveErrors
    {
    }

    /// <summary>
    /// Represents bootstrapper with dependency registrator.
    /// </summary>
    public interface IBootstrapperWithRegistrator : IBootstrapper, IHaveRegistrator,
        IExtensible<IBootstrapperWithRegistrator>
    {
    }

    /// <summary>
    /// Represents bootstrapper with ioc container adapter.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <seealso cref="IBootstrapper" />
    public interface IBootstrapperWithContainerAdapter<TIocContainerAdapter> :
        IBootstrapperWithRegistrator, IExtensible<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>
        where TIocContainerAdapter : IIocContainer
    {
    }

    /// <summary>
    /// Represents bootstrapper with ioc container and ioc container adapter.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>
    /// <seealso cref="IBootstrapperWithContainerAdapter{TIocContainerAdapter}" />
    public interface IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> :
        IBootstrapperWithContainerAdapter<TIocContainerAdapter>, IHaveContainer<TIocContainer>,
        IExtensible<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>>
        where TIocContainerAdapter : IIocContainer
    {
    }
}
