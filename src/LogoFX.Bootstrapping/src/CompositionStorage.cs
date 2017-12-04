using System.Collections.Concurrent;
using Solid.Practices.Modularity;

namespace LogoFX.Bootstrapping
{
    static class CompositionStorage
    {
        private static readonly ConcurrentDictionary<string, ICompositionModule[]> InternalStorage =
            new ConcurrentDictionary<string, ICompositionModule[]>();

        internal static void AddCompositionModules(string rootPath, ICompositionModule[] compositionModules)
        {
            InternalStorage.TryAdd(rootPath, compositionModules);
        }

        internal static ICompositionModule[] GetCompositionModules(string rootPath)
        {
            ICompositionModule[] compositionModules;
            InternalStorage.TryGetValue(rootPath, out compositionModules);
            return compositionModules;
        }
    }
}