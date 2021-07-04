using Solid.Bootstrapping;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

// ReSharper disable once CheckNamespace
namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers composition modules into the bootstrapper's ioc container.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TIocContainerAdapter, TIocContainer> :
        IMiddleware<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>>        
        where TIocContainerAdapter : class, IIocContainer
        where TIocContainer : class
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> Apply(
            IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> @object)
        {
            var internalMiddleware =
                new RegisterContainerCompositionModulesMiddleware
                    <IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>, TIocContainer>();
            return internalMiddleware.Apply(@object);
        }
    }    
}