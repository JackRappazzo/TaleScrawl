using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;

namespace TaleScrawl.UnitTests
{
    public abstract class WhenTestingWallRasterizer : ComposableTestingTheBehaviourOf
    {
        [ItemUnderTest]
        protected WallRasterizer Rasterizer = null!;
    }
}
