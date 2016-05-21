using System;
using System.Collections.Generic;
using System.Reflection;
using Solid.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

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
    /// Represents the bootstrapper.
    /// </summary>
    public interface IBootstrapper : IInitializable, IInitializationInfo, IExtensible<IBootstrapper>
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
    }

    /// <summary>
    /// Represents bootstrapper with ioc container registrator.
    /// </summary>
    public interface IBootstrapperWithContainerRegistrator : IBootstrapper, IHaveContainerRegistrator, IExtensible<IBootstrapperWithContainerRegistrator>
    {
        
    }

    /// <summary>
    /// Represents bootstrapper with ioc container adapter.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <seealso cref="IBootstrapper" />
    public interface IBootstrapperWithContainerAdapter<TIocContainerAdapter> : 
        IBootstrapperWithContainerRegistrator, IExtensible<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>
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
