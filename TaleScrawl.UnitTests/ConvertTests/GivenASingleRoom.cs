using System;
using System.Collections.Generic;
using System.Linq;
using TaleScrawl.UnitTests;
using TaleSpire.Slab;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.ConvertTests
{
    public class GivenASingleRoom : WhenTestingBuildSlab
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ADocumentWithOneFourByFourRoomIsProvided)
            .When(BuildSlabIsCalled)
            .Then(ShouldContainFloorWallAndCornerLayouts);

        [Given]
        public void ADocumentWithOneFourByFourRoomIsProvided()
        {
            Document = TestDocuments.FourByFourRoom();
        }

        [Then]
        public void ShouldContainFloorWallAndCornerLayouts()
        {
            IReadOnlyList<ILayout> layouts = Result.Layouts;

            Assert.That(layouts, Has.Count.EqualTo(3));

            Assert.That(layouts[0].AssetKindId, Is.EqualTo(Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768")));
            Assert.That(layouts[0].AssetCount, Is.EqualTo(16));
            Assert.That(layouts[1].AssetCount, Is.EqualTo(16));
            Assert.That(layouts[2].AssetCount, Is.EqualTo(4));

            List<Float3> floor = layouts[0].Assets.Select(a => a.Position).ToList();
            Assert.That(floor, Has.Count.EqualTo(16));
            Assert.That(floor, Has.All.Matches<Float3>(p => p.X >= 0 && p.Z >= 0));
            Assert.That(floor[0], Is.EqualTo(new Float3(1, 0, 4)));
            Assert.That(floor[^1], Is.EqualTo(new Float3(4, 0, 1)));
        }
    }
}
