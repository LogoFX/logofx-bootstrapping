using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Represents a composition module, which may register dependencies using the bootstrapper.
    /// </summary>
    /// <seealso cref="ICompositionModule" />
    public interface IBootstrapperCompositionModule : ICompositionModule
    {
        /// <summary>
        /// Registers dependencies using the bootstrapper.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        void RegisterModule(IBootstrapperWithContainerRegistrator bootstrapper);
    }
}