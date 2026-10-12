using TaleScrawl.Core.Ds;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;

namespace TaleScrawl.UnitTests.Ds
{
    public abstract class WhenTestingDsFileReader : ComposableTestingTheBehaviourOf
    {
        [ItemUnderTest]
        protected DsFileReader Reader = null!;
    }
}
