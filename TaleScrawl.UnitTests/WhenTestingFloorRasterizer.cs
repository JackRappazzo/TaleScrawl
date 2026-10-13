using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;

namespace TaleScrawl.UnitTests
{
    public abstract class WhenTestingFloorRasterizer : ComposableTestingTheBehaviourOf
    {
        [ItemUnderTest]
        protected FloorRasterizer Rasterizer = null!;
    }
}
