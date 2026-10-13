using System;
using System.Collections.Generic;
using TaleScrawl.Core;
using TaleScrawl.Core.Options;
using TaleScrawl.UnitTests;
using TaleSpire.Slab;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.ConvertTests
{
    public class GivenFloorOnlyIsRequested : WhenTestingBuildSlab
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(AFloorOnlyConversionIsRequested)
            .When(BuildSlabIsCalled)
            .Then(ShouldProduceOnlyTheFloorLayout);

        [Given]
        public void AFloorOnlyConversionIsRequested()
        {
            Converter = new TaleScrawlConverter(new SlabOptions { IncludeWalls = false });
            Document = TestDocuments.FourByFourRoom();
        }

        [Then]
        public void ShouldProduceOnlyTheFloorLayout()
        {
            IReadOnlyList<ILayout> layouts = Result.Layouts;

            Assert.That(layouts, Has.Count.EqualTo(1));
            Assert.That(layouts[0].AssetKindId, Is.EqualTo(Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768")));
            Assert.That(layouts[0].AssetCount, Is.EqualTo(16));
        }
    }
}
