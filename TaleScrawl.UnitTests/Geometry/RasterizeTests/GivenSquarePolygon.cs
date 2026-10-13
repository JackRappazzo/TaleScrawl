using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeTests
{
    public class GivenSquarePolygon : WhenTestingRasterize
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ASingleFourByFourSquareIsProvided)
            .When(RasterizeIsCalled)
            .Then(ShouldReturnAllSixteenCells);

        [Given]
        public void ASingleFourByFourSquareIsProvided()
        {
            Polygons = new List<Polygon>
            {
                new(new List<IReadOnlyList<Point>>
                {
                    new List<Point> { new(0, 0), new(4, 0), new(4, 4), new(0, 4) }
                })
            };
        }

        [Then]
        public void ShouldReturnAllSixteenCells()
        {
            Assert.That(Result, Has.Count.EqualTo(16));
            Assert.That(Result, Does.Contain((0, 0)));
            Assert.That(Result, Does.Contain((3, 3)));
            Assert.That(Result, Does.Not.Contain((4, 4)));
        }
    }
}
