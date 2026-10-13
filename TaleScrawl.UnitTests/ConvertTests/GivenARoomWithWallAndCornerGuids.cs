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
    public class GivenARoomWithWallAndCornerGuids : WhenTestingBuildSlab
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ADocumentWithARoomAndWallAndCornerGuidsIsProvided)
            .When(BuildSlabIsCalled)
            .Then(ShouldEncodeFloorWallAndCornerLayouts);

        [Given]
        public void ADocumentWithARoomAndWallAndCornerGuidsIsProvided()
        {
            Converter = new TaleScrawlConverter(new SlabOptions
            {
                WallGuid = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                CornerGuid = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
            });

            Document = TestDocuments.FourByFourRoom();
        }

        [Then]
        public void ShouldEncodeFloorWallAndCornerLayouts()
        {
            IReadOnlyList<ILayout> layouts = Result.Layouts;

            Assert.That(layouts, Has.Count.EqualTo(3));
            Assert.That(layouts[0].AssetKindId, Is.EqualTo(Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768")));
            Assert.That(layouts[1].AssetKindId, Is.EqualTo(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")));
            Assert.That(layouts[2].AssetKindId, Is.EqualTo(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")));
            Assert.That(layouts[0].AssetCount, Is.EqualTo(16));
            Assert.That(layouts[1].AssetCount, Is.EqualTo(16));
            Assert.That(layouts[2].AssetCount, Is.EqualTo(4));
        }
    }
}
