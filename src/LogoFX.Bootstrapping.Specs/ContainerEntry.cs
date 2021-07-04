using System;

namespace LogoFX.Bootstrapping.Specs
{
    class ContainerEntry
    {
        public ContainerEntry(
            Type interfaceType,
            Type implementationType, 
            bool isSingleton)
        {
            InterfaceType = interfaceType;
            ImplementationType = implementationType;
            IsSingleton = isSingleton;
        }

        public Type InterfaceType { get; private set; }
        public Type ImplementationType { get; private set; }
        public bool IsSingleton { get; private set; }
    }
}