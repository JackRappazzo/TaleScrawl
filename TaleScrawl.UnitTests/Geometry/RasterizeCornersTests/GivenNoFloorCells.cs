using System.Collections.Generic;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeCornersTests
{
    public class GivenNoFloorCells : WhenTestingRasterizeCorners
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(NoFloorCellsAreProvided)
            .When(RasterizeCornersIsCalled)
            .Then(ShouldReturnNoCells);

        [Given]
        public void NoFloorCellsAreProvided()
        {
            FloorCells = new HashSet<(int, int)>();
        }

        [Then]
        public void ShouldReturnNoCells()
        {
            Assert.That(Result, Is.Empty);
        }
    }
}
