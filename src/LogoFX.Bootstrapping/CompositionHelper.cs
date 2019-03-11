using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Container;
using Solid.Practices.Composition.Contracts;
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
        /// <param name="key"></param>
        /// <param name="assemblySourceProvider">The assembly source provider.</param>
        /// <param name="reuseCompositionInformation">True, if the composition information should be reused, false otherwise.</param>        
        /// <returns></returns>
        public static CompositionInfo GetCompositionInfo(
            string key,
            IAssemblySourceProvider assemblySourceProvider,
            bool reuseCompositionInformation)
        {                               
            CompositionInfo compositionInfo;
            if (reuseCompositionInformation == false)
            {
                compositionInfo = ConstructCompositionInfo(assemblySourceProvider.Assemblies);
            }
            else
            {
                compositionInfo = new CompositionInfo {Modules = CompositionStorage.GetCompositionModules(key)};
                if (compositionInfo.Modules != null)
                {
                    return compositionInfo;
                }
                compositionInfo = ConstructCompositionInfo(assemblySourceProvider.Assemblies);
                CompositionStorage.AddCompositionModules(key, compositionInfo.Modules.ToArray());
                return compositionInfo;
            }
            return compositionInfo;
        }

        private static CompositionInfo ConstructCompositionInfo(
            string relativePath,
            string[] prefixes)
        {
            var compositionManager = new CompositionManager();
            var discoveryAspect = new DiscoveryAspect(new CompositionOptions
                {Prefixes = prefixes, RelativePath = relativePath});
            var compositionInfo = new CompositionInfo();            
            try
            {
                compositionManager.Initialize(discoveryAspect.Assemblies);
            }
            catch (AggregateAssemblyInspectionException exception)
            {
                compositionInfo.Errors = exception.InnerExceptions;
            }
            finally
            {
                compositionInfo.Modules = compositionManager.Modules == null
                    ? new ICompositionModule[] { }
                    : compositionManager.Modules.ToArray();
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