using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeWallsTests
{
    public abstract class WhenTestingRasterizeWalls : WhenTestingWallRasterizer
    {
        protected HashSet<(int X, int Y)> FloorCells = null!;
        protected IReadOnlyList<TilePlacement> Result = null!;

        [When]
        public void RasterizeWallsIsCalled()
        {
            Result = Rasterizer.RasterizeWalls(FloorCells);
        }
    }
}
