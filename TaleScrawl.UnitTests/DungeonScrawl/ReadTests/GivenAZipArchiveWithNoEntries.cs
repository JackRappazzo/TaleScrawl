using System.IO;
using System.IO.Compression;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.DungeonScrawl.ReadTests
{
    public class GivenAZipArchiveWithNoEntries : WhenTestingRead
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(AZipArchiveWithNoEntriesIsProvided)
            .When(ReadIsCalled)
            .Then(ShouldThrowInvalidDataException);

        [Given]
        public void AZipArchiveWithNoEntriesIsProvided()
        {
            var memory = new MemoryStream();
            using (var archive = new ZipArchive(memory, ZipArchiveMode.Create, leaveOpen: true))
            {
            }

            memory.Position = 0;
            Input = memory;
        }

        [Then]
        public void ShouldThrowInvalidDataException()
        {
            Assert.That(ThrownException, Is.TypeOf<InvalidDataException>());
        }
    }
}
