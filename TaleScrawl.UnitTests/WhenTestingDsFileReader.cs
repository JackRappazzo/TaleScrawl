using TaleScrawl.Core.DungeonScrawl;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;

namespace TaleScrawl.UnitTests
{
    public abstract class WhenTestingDsFileReader : ComposableTestingTheBehaviourOf
    {
        [ItemUnderTest]
        protected DsFileReader Reader = null!;
    }
}
