using System;
using Solid.Bootstrapping;
using Solid.Practices.Middleware;

// ReSharper disable once CheckNamespace
namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Registers collection of services. This is used in case of 
    /// loosely coupled modular application where the services are defined in separate assemblies 
    /// and/or are otherwise private.
    /// </summary>
    public class RegisterCollectionMiddleware :
        IMiddleware<IBootstrapperWithRegistrator>
    {
        private readonly Type _serviceContractType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCollectionMiddleware"/> class.
        /// </summary>
        /// <param name="serviceContractType">The type of the module root object.</param>
        public RegisterCollectionMiddleware(Type serviceContractType)
        {
            _serviceContractType = serviceContractType;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public IBootstrapperWithRegistrator
            Apply(IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware =
                new RegisterCollectionMiddleware<IBootstrapperWithRegistrator>(_serviceContractType);
            return internalMiddleware.Apply(@object);
        }
    }        
}