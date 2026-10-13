using TaleScrawl.Core;
using TaleScrawl.Core.Options;
using TaleScrawl.UnitTests;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.ConvertTests
{
    public class GivenWrapInBackticksIsRequested : WhenTestingConvert
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(AConversionWrappedInBackticksIsRequested)
            .When(ConvertIsCalled)
            .Then(ShouldWrapTheSlabInBackticks);

        [Given]
        public void AConversionWrappedInBackticksIsRequested()
        {
            Converter = new TaleScrawlConverter(new SlabOptions { WrapInBackticks = true });
            Document = TestDocuments.FourByFourRoom();
        }

        [Then]
        public void ShouldWrapTheSlabInBackticks()
        {
            Assert.That(Result, Does.StartWith("```"));
            Assert.That(Result, Does.EndWith("```"));
        }
    }
}
