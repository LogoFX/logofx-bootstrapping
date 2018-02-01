using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Solid.Practices.Composition;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Helper for efficient composition modules management
    /// </summary>
    public static class CompositionHelper
    {
        /// <summary>
        /// Gets the collection of <see cref="ICompositionModule"/>
        /// </summary>
        /// <param name="modulesPath">The relative modules path.</param>
        /// <param name="prefixes">The prefixes to be looked for</param>
        /// <param name="reuseCompositionInformation">True, if the composition modules information should be reused, false otherwise</param>
        /// <returns></returns>
        public static IEnumerable<ICompositionModule> GetCompositionModules(
            string modulesPath,
            string[] prefixes,
            bool reuseCompositionInformation)
        {
            var rootPath = PlatformProvider.Current.GetAbsolutePath(modulesPath);
            var key = rootPath;
            ICompositionModule[] compositionModules;
            if (reuseCompositionInformation == false)
            {
                compositionModules = CreateCompositionModules(rootPath, prefixes);
            }
            else
            {
                compositionModules = CompositionStorage.GetCompositionModules(key);
                if (compositionModules != null)
                {
                    return compositionModules;
                }
                compositionModules = CreateCompositionModules(rootPath, prefixes).ToArray();
                CompositionStorage.AddCompositionModules(key, compositionModules);
                return compositionModules;
            }
            return compositionModules;
        }

        private static ICompositionModule[] CreateCompositionModules(
            string modulesPath,
            string[] prefixes)
        {
            var compositionManager = new CompositionManager();
            compositionManager.Initialize(modulesPath, prefixes);
            return compositionManager.Modules.ToArray();
        }

        /// <summary>
        /// Gets the collection of <see cref="ICompositionModule"/>
        /// </summary>
        /// <param name="assemblies">The collection of assemblies.</param>
        /// <param name="reuseCompositionInformation">True, if the composition modules information should be reused, false otherwise</param>
        /// <returns></returns>
        public static IEnumerable<ICompositionModule> GetCompositionModules(
            IEnumerable<Assembly> assemblies,
            bool reuseCompositionInformation)
        {
            ICompositionModule[] compositionModules;
            if (reuseCompositionInformation == false)
            {
                compositionModules = CreateCompositionModules(assemblies);
            }
            else
            {
                const string key = "key";
                compositionModules = CompositionStorage.GetCompositionModules(key);
                if (compositionModules != null)
                {
                    return compositionModules;
                }
                compositionModules = CreateCompositionModules(assemblies).ToArray();
                CompositionStorage.AddCompositionModules(key, compositionModules);
                return compositionModules;
            }
            return compositionModules;
        }

        private static ICompositionModule[] CreateCompositionModules(
            IEnumerable<Assembly> assemblies)
        {
            var compositionManager = new CompositionManager();
            compositionManager.Initialize(assemblies);
            return compositionManager.Modules.ToArray();
        }
    }
}