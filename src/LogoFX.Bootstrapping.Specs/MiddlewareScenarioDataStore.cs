using Attest.Testing.Context.SpecFlow;
using TechTalk.SpecFlow;

namespace LogoFX.Bootstrapping.Specs
{
    internal sealed class MiddlewareScenarioDataStore : ScenarioDataStoreBase
    {
        public MiddlewareScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public FakeIocContainer ContainerAdapter
        {
            get => GetValue<FakeIocContainer>();
            set => SetValue(value);
        }

        public FakeContainer Container
        {
            get => GetValue<FakeContainer>();
            set => SetValue(value);
        }

        public object Bootstrapper
        {
            get => GetValue<object>();
            set => SetValue(value);
        }
    }
}
