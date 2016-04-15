using System.Linq;
using System.Reflection;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers the core application components (root object, ioc container etc.) into the ioc container adapter.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCoreMiddleware<TRootObject, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> @object)
        {
            @object.ContainerAdapter.RegisterCore<TRootObject, TIocContainerAdapter>();
            return @object;
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container adapter.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TRootObject, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> @object)
        {
            @object.ContainerAdapter.RegisterContainerAdapterCompositionModules(@object.Modules);
            return @object;
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TRootObject, TIocContainerAdapter, TIocContainer> :
        IMiddleware<IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer
        where TIocContainer : class
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer> Apply(
            IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer> @object)
        {
            @object.RegisterContainerCompositionModules(@object.Modules);
            return @object;
        }
    }

    /// <summary>
    /// Registers the root objects of the modules. This is used in case of 
    /// loosely coupled module-oriented application where the modules have their own dependencies 
    /// that need to be injected during their creation.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    /// <typeparam name="TModuleRootObject">The type of the module root object.</typeparam>    
    public class RegisterModuleRootObjectsMiddleware<TRootObject, TIocContainerAdapter, 
        TModuleRootObject> : 
        IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>>
        where TIocContainerAdapter : IIocContainer
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>
            Apply(IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> @object)
        {
            var moduleRootObjectType = typeof (TModuleRootObject);
            var typeInfo = moduleRootObjectType.GetTypeInfo();
            var moduleTypes = @object.Assemblies.Select(t => t.DefinedTypes.ToArray()).SelectMany(k => k).Where(t =>
                t.IsInterface == false && t.IsAbstract == false &&
                typeInfo.IsAssignableFrom(t)).Select(t => t.AsType());
            @object.ContainerAdapter.RegisterCollection(moduleRootObjectType, moduleTypes);
            return @object;
        }
    }
}
