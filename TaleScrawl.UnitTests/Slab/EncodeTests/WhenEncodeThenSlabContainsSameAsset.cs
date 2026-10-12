using System;
using System.Collections.Generic;
using TaleScrawl.Core.Slab;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Slab.EncodeTests
{
    public class WhenEncodeThenSlabContainsSameAsset : WhenTestingEncode
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ASingleAssetIsProvided)
            .When(EncodeIsCalled)
            .Then(ShouldContainTheSameAsset);

        [Given]
        public void ASingleAssetIsProvided()
        {
            AssetKindId = Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768");
            Assets = new List<AssetPlacement>
            {
                new(0, 0, 0)
            };
        }

        [Then]
        public void ShouldContainTheSameAsset()
        {
            var decoder = new SlabDecoder();
            IReadOnlyList<SlabLayout> layouts = decoder.Decode(Result);

            Assert.That(layouts, Has.Count.EqualTo(1));
            Assert.That(layouts[0].AssetKindId, Is.EqualTo(AssetKindId));
            Assert.That(layouts[0].Assets, Has.Count.EqualTo(1));
            Assert.That(layouts[0].Assets[0].X, Is.EqualTo(0).Within(0.001));
            Assert.That(layouts[0].Assets[0].Y, Is.EqualTo(0).Within(0.001));
            Assert.That(layouts[0].Assets[0].Z, Is.EqualTo(0).Within(0.001));
        }
    }
}
