using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using TaleScrawl.Core.Geometry;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.DungeonScrawl.ReadTests
{
    public class GivenAZipArchiveContainingAJsonDocument : WhenTestingRead
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(AZipArchiveContainingAJsonDocumentIsProvided)
            .When(ReadIsCalled)
            .Then(ShouldContainTheExpectedGeometry);

        [Given]
        public void AZipArchiveContainingAJsonDocumentIsProvided()
        {
            const string json = @"
            {
              ""data"": {
                ""geometry"": {
                  ""g1"": {
                    ""polygons"": [[[[0,0],[4,0],[4,4],[0,4]]]]
                  }
                }
              }
            }";

            Input = CreateZipStream(json);
        }

        [Then]
        public void ShouldContainTheExpectedGeometry()
        {
            Assert.That(Result.Data, Is.Not.Null);
            Assert.That(Result.Data!.Geometry, Has.Count.EqualTo(1));
            Assert.That(Result.Data.Geometry, Does.ContainKey("g1"));

            List<List<List<Point>>>? polygons = Result.Data.Geometry!["g1"].Polygons;
            Assert.That(polygons, Has.Count.EqualTo(1));
            Assert.That(polygons![0], Has.Count.EqualTo(1));
            Assert.That(polygons[0][0], Has.Count.EqualTo(4));
            Assert.That(polygons[0][0][0], Is.EqualTo(new Point(0, 0)));
        }

        private static Stream CreateZipStream(string json)
        {
            var memory = new MemoryStream();
            using (var archive = new ZipArchive(memory, ZipArchiveMode.Create, leaveOpen: true))
            {
                ZipArchiveEntry entry = archive.CreateEntry("map");
                using var writer = new StreamWriter(entry.Open());
                writer.Write(json);
            }

            memory.Position = 0;
            return memory;
        }
    }
}
