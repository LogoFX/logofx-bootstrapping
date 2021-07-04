using System.Linq;
using Solid.Practices.Middleware;

// ReSharper disable once CheckNamespace
namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers the collection of <see cref="IBootstrapperCompositionModule"/> modules.
    /// </summary>
    /// <seealso cref="Solid.Practices.Middleware.IMiddleware{IBootstrapperWithRegistrator}" />
    public class RegisterBootstrapperCompositionModulesMiddleware : IMiddleware<IBootstrapperWithRegistrator>
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public IBootstrapperWithRegistrator Apply(IBootstrapperWithRegistrator @object)
        {
            foreach (var module in @object.Modules.OfType<IBootstrapperCompositionModule>())
            {
                module.RegisterModule(@object);
            }
            return @object;
        }
    }
}