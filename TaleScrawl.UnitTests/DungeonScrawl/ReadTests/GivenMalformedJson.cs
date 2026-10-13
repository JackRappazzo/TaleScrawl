using System.IO;
using System.Text;
using System.Text.Json;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.DungeonScrawl.ReadTests
{
    public class GivenMalformedJson : WhenTestingRead
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(AMalformedJsonDocumentIsProvided)
            .When(ReadIsCalled)
            .Then(ShouldThrowJsonException);

        [Given]
        public void AMalformedJsonDocumentIsProvided()
        {
            Input = new MemoryStream(Encoding.UTF8.GetBytes("{ this is not valid json"));
        }

        [Then]
        public void ShouldThrowJsonException()
        {
            Assert.That(ThrownException, Is.TypeOf<JsonException>());
        }
    }
}
