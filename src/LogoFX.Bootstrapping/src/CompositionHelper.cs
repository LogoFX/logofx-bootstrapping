using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Container;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    /// <summary>
    /// Represents composition information.
    /// </summary>
    public struct CompositionInfo
    {
        /// <summary>
        /// Gets the collection of <see cref="ICompositionModule"/>.
        /// </summary>
        public IEnumerable<ICompositionModule> Modules { get; internal set; }

        /// <summary>
        /// Gets the collection of composition errors.
        /// </summary>
        public IEnumerable<Exception> Errors { get; internal set; }
    }

    /// <summary>
    /// Helper for efficient composition modules management
    /// </summary>
    public static class CompositionHelper
    {
        /// <summary>
        /// Gets the composition information.
        /// </summary>
        /// <param name="modulesPath">The relative modules path.</param>
        /// <param name="prefixes">The prefixes to be looked for</param>
        /// <param name="reuseCompositionInformation">True, if the composition information should be reused, false otherwise.</param>
        /// <returns></returns>
        public static CompositionInfo GetCompositionInfo(
            string modulesPath,
            string[] prefixes,
            bool reuseCompositionInformation)
        {
            var rootPath = PlatformProvider.Current.GetAbsolutePath(modulesPath);
            var key = rootPath;
            CompositionInfo compositionInfo;
            if (reuseCompositionInformation == false)
            {
                compositionInfo = ConstructCompositionInfo(rootPath, prefixes);
            }
            else
            {
                compositionInfo = new CompositionInfo {Modules = CompositionStorage.GetCompositionModules(key)};
                if (compositionInfo.Modules != null)
                {
                    return compositionInfo;
                }
                compositionInfo = ConstructCompositionInfo(rootPath, prefixes);
                CompositionStorage.AddCompositionModules(key, compositionInfo.Modules.ToArray());
                return compositionInfo;
            }
            return compositionInfo;
        }

        private static CompositionInfo ConstructCompositionInfo(
            string modulesPath,
            string[] prefixes)
        {
            var compositionManager = new CompositionManager();
            var compositionInfo = new CompositionInfo();            
            try
            {
                compositionManager.Initialize(modulesPath, prefixes);
            }
            catch (AggregateAssemblyInspectionException exception)
            {
                compositionInfo.Errors = exception.InnerExceptions;
            }
            finally
            {
                compositionInfo.Modules = compositionManager.Modules.ToArray();
            }

            return compositionInfo;
        }

        /// <summary>
        /// Gets the composition information.
        /// </summary>
        /// <param name="assemblies">The collection of assemblies.</param>
        /// <param name="reuseCompositionInformation">True, if the composition information should be reused, false otherwise</param>
        /// <returns></returns>
        public static CompositionInfo GetCompositionInfo(
            IEnumerable<Assembly> assemblies,
            bool reuseCompositionInformation)
        {         
            CompositionInfo compositionInfo;
            if (reuseCompositionInformation == false)
            {
                compositionInfo = ConstructCompositionInfo(assemblies);
            }
            else
            {
                const string key = "key";
                compositionInfo = new CompositionInfo {Modules = CompositionStorage.GetCompositionModules(key)};
                if (compositionInfo.Modules != null)
                {
                    return compositionInfo;
                }
                compositionInfo = ConstructCompositionInfo(assemblies);
                CompositionStorage.AddCompositionModules(key, compositionInfo.Modules.ToArray());
                return compositionInfo;
            }
            return compositionInfo;
        }

        private static CompositionInfo ConstructCompositionInfo(
            IEnumerable<Assembly> assemblies)
        {
            var compositionManager = new CompositionManager();
            var compositionInfo = new CompositionInfo();
            try
            {
                compositionManager.Initialize(assemblies);
            }
            catch (AggregateAssemblyInspectionException exception)
            {
                compositionInfo.Errors = exception.InnerExceptions;
            }
            finally
            {
                compositionInfo.Modules = compositionManager.Modules.ToArray();
            }

            return compositionInfo;
        }
    }
}