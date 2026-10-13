using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeTests
{
    public class GivenOverlappingPolygons : WhenTestingRasterize
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(TwoOverlappingPolygonsAreProvided)
            .When(RasterizeIsCalled)
            .Then(ShouldDeduplicateOverlappingCells);

        [Given]
        public void TwoOverlappingPolygonsAreProvided()
        {
            Polygons = new List<Polygon>
            {
                new(new List<IReadOnlyList<Point>>
                {
                    new List<Point> { new(0, 0), new(2, 0), new(2, 2), new(0, 2) }
                }),
                new(new List<IReadOnlyList<Point>>
                {
                    new List<Point> { new(1, 1), new(3, 1), new(3, 3), new(1, 3) }
                })
            };
        }

        [Then]
        public void ShouldDeduplicateOverlappingCells()
        {
            Assert.That(Result, Has.Count.EqualTo(7));
        }
    }
}
