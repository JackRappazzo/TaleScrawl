using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Geometry.RasterizeTests
{
    public class GivenNoPolygons : WhenTestingRasterize
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(NoPolygonsAreProvided)
            .When(RasterizeIsCalled)
            .Then(ShouldReturnNoCells);

        [Given]
        public void NoPolygonsAreProvided()
        {
            Polygons = new List<Polygon>();
        }

        [Then]
        public void ShouldReturnNoCells()
        {
            Assert.That(Result, Is.Empty);
        }
    }
}
