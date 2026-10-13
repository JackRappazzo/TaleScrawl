using System;
using TaleScrawl.UnitTests;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.ConvertTests
{
    public class GivenAMapWithNoFloorCells : WhenTestingBuildSlab
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ADocumentWithNoGeometryIsProvided)
            .When(BuildSlabIsCalled)
            .Then(ShouldThrowInvalidOperationException);

        [Given]
        public void ADocumentWithNoGeometryIsProvided()
        {
            Document = TestDocuments.Empty();
        }

        [Then]
        public void ShouldThrowInvalidOperationException()
        {
            Assert.That(ThrownException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
