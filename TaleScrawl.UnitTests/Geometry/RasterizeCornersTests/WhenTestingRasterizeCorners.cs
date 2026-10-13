using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeCornersTests
{
    public abstract class WhenTestingRasterizeCorners : WhenTestingWallRasterizer
    {
        protected HashSet<(int X, int Y)> FloorCells = null!;
        protected IReadOnlyList<TilePlacement> Result = null!;

        [When]
        public void RasterizeCornersIsCalled()
        {
            Result = Rasterizer.RasterizeCorners(FloorCells);
        }
    }
}
