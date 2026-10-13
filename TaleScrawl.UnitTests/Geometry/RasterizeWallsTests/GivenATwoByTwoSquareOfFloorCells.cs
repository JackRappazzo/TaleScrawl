using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeWallsTests
{
    public class GivenATwoByTwoSquareOfFloorCells : WhenTestingRasterizeWalls
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ASingleTwoByTwoSquareOfFloorCellsIsProvided)
            .When(RasterizeWallsIsCalled)
            .Then(ShouldReturnEightPerimeterCellsFacingTheFloor);

        [Given]
        public void ASingleTwoByTwoSquareOfFloorCellsIsProvided()
        {
            FloorCells = new HashSet<(int, int)>
            {
                (0, 0), (1, 0), (0, 1), (1, 1)
            };
        }

        [Then]
        public void ShouldReturnEightPerimeterCellsFacingTheFloor()
        {
            Assert.That(Result, Has.Count.EqualTo(8));
            Assert.That(Result, Is.EqualTo(new[]
            {
                new TilePlacement(0, -1, 1),
                new TilePlacement(1, -1, 1),
                new TilePlacement(-1, 0, 0),
                new TilePlacement(2, 0, 2),
                new TilePlacement(-1, 1, 0),
                new TilePlacement(2, 1, 2),
                new TilePlacement(0, 2, 3),
                new TilePlacement(1, 2, 3),
            }));
        }
    }
}
