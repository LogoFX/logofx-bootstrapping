using System.Collections.Generic;

namespace LogoFX.Bootstrapping.Specs
{
    class FakeContainer : IRegistrationCollection
    {
        private readonly List<ContainerEntry> _registrations = new List<ContainerEntry>();

        private readonly List<InstanceEntry> _instances = new List<InstanceEntry>();

        IEnumerable<ContainerEntry> IRegistrationCollection.Registrations
        {
            get { return _registrations; }
        }

        internal IEnumerable<InstanceEntry> Instances
        {
            get { return _instances; }
        }

        public void RegisterTransient<TService, TImplementation>()
        {
            _registrations.Add(new ContainerEntry(typeof (TService), typeof (TImplementation), false));
        }
    }
}