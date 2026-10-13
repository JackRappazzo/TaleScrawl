using System.IO;
using System.Text;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.DungeonScrawl.ReadTests
{
    public class GivenPlainJsonFile : WhenTestingRead
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(APlainJsonDocumentIsProvided)
            .When(ReadIsCalled)
            .Then(ShouldReturnTheDeserializedDocument);

        [Given]
        public void APlainJsonDocumentIsProvided()
        {
            const string json = @"
            {
              ""data"": {
                ""geometry"": {
                  ""g1"": {
                    ""polygons"": [[[[0,0],[2,0],[2,2],[0,2]]]]
                  }
                }
              }
            }";

            Input = new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

        [Then]
        public void ShouldReturnTheDeserializedDocument()
        {
            Assert.That(Result.Data, Is.Not.Null);
            Assert.That(Result.Data!.Geometry, Does.ContainKey("g1"));
        }
    }
}
