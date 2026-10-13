using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeCornersTests
{
    public class GivenATwoByTwoSquareOfFloorCells : WhenTestingRasterizeCorners
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ASingleTwoByTwoSquareOfFloorCellsIsProvided)
            .When(RasterizeCornersIsCalled)
            .Then(ShouldReturnTheFourDiagonalCornerCells);

        [Given]
        public void ASingleTwoByTwoSquareOfFloorCellsIsProvided()
        {
            FloorCells = new HashSet<(int, int)>
            {
                (0, 0), (1, 0), (0, 1), (1, 1)
            };
        }

        [Then]
        public void ShouldReturnTheFourDiagonalCornerCells()
        {
            Assert.That(Result, Has.Count.EqualTo(4));
            Assert.That(Result, Is.EqualTo(new[]
            {
                new TilePlacement(-1, -1, 1),
                new TilePlacement(2, -1, 2),
                new TilePlacement(-1, 2, 0),
                new TilePlacement(2, 2, 3),
            }));
        }
    }
}
