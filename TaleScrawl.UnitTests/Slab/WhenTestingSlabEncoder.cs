using TaleScrawl.Core.Slab;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;

namespace TaleScrawl.UnitTests.Slab
{
    public abstract class WhenTestingSlabEncoder : ComposableTestingTheBehaviourOf
    {
        [ItemUnderTest]
        protected SlabEncoder Encoder = null!;
    }
}
