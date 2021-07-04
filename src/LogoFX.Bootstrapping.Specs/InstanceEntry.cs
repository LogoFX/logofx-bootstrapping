using System;

namespace LogoFX.Bootstrapping.Specs
{
    class InstanceEntry
    {
        public InstanceEntry(Type instanceType, object instance)
        {
            InstanceType = instanceType;
            Instance = instance;
        }

        public Type InstanceType { get; private set; }
        public object Instance { get; private set; }
    }
}