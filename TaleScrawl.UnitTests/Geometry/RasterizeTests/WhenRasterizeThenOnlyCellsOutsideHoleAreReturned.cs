using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeTests
{
    public class WhenRasterizeThenOnlyCellsOutsideHoleAreReturned : WhenTestingRasterize
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(APolygonWithAHoleIsProvided)
            .When(RasterizeIsCalled)
            .Then(ShouldReturnOnlyTheCellsOutsideTheHole);

        [Given]
        public void APolygonWithAHoleIsProvided()
        {
            Polygons = new List<Polygon>
            {
                new(new List<IReadOnlyList<Point>>
                {
                    new List<Point>
                    {
                        new(0, 0), new(4, 0), new(4, 4), new(0, 4)
                    },
                    new List<Point>
                    {
                        new(1, 1), new(3, 1), new(3, 3), new(1, 3)
                    }
                })
            };
        }

        [Then]
        public void ShouldReturnOnlyTheCellsOutsideTheHole()
        {
            Assert.That(Result, Has.Count.EqualTo(12));
            Assert.That(Result, Does.Contain((0, 0)));
            Assert.That(Result, Does.Not.Contain((1, 1)));
            Assert.That(Result, Does.Not.Contain((2, 2)));
        }
    }
}
