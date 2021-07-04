using Solid.Bootstrapping;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

// ReSharper disable once CheckNamespace
namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers the dependency resolver.
    /// </summary>    
    public class RegisterResolverMiddleware : IMiddleware<IBootstrapperWithRegistrator>
    {
        private readonly IDependencyResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterResolverMiddleware"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public RegisterResolverMiddleware(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>Applies the middleware on the specified object.</summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware = new RegisterResolverMiddleware<IBootstrapperWithRegistrator>(_resolver);
            return internalMiddleware.Apply(@object);
        }
    }        
}
