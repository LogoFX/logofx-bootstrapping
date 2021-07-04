using System.Collections.Generic;

namespace LogoFX.Bootstrapping.Specs
{
    interface IRegistrationCollection
    {
        IEnumerable<ContainerEntry> Registrations { get; }
    }
}