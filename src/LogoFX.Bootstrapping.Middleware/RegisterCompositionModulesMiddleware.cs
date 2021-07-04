using Solid.Bootstrapping;
using Solid.Practices.Middleware;

// ReSharper disable once CheckNamespace
namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers composition modules into the bootstrapper's registrator.
    /// </summary>  
    public class RegisterCompositionModulesMiddleware : IMiddleware<IBootstrapperWithRegistrator>        
    {
        /// <summary>Applies the middleware on the specified object.</summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware = new RegisterCompositionModulesMiddleware<IBootstrapperWithRegistrator>();
            return internalMiddleware.Apply(@object);
        }
    }    
}