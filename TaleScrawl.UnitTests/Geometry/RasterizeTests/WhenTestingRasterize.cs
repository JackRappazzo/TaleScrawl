using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeTests
{
    public abstract class WhenTestingRasterize : WhenTestingFloorRasterizer
    {
        protected List<Polygon> Polygons = null!;
        protected HashSet<(int X, int Y)> Result = null!;

        [When]
        public void RasterizeIsCalled()
        {
            Result = Rasterizer.Rasterize(Polygons);
        }
    }
}
