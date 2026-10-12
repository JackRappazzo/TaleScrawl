using System;
using System.Collections.Generic;
using TaleScrawl.Core.Slab;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Slab.EncodeTests
{
    public class WhenEncodeThenSlabContainsAllAssetsInOrder : WhenTestingEncode
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(SeveralAssetsAreProvided)
            .When(EncodeIsCalled)
            .Then(ShouldContainAllAssetsInOrder);

        [Given]
        public void SeveralAssetsAreProvided()
        {
            AssetKindId = Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768");
            Assets = new List<AssetPlacement>
            {
                new(0, 0, 0),
                new(1, 0, 0),
                new(2, 0, 1),
                new(3.5, 0, 2.25),
                new(4, 0, 3)
            };
        }

        [Then]
        public void ShouldContainAllAssetsInOrder()
        {
            var decoder = new SlabDecoder();
            IReadOnlyList<SlabLayout> layouts = decoder.Decode(Result);

            Assert.That(layouts, Has.Count.EqualTo(1));
            Assert.That(layouts[0].Assets, Has.Count.EqualTo(Assets.Count));

            for (int i = 0; i < Assets.Count; i++)
            {
                Assert.That(layouts[0].Assets[i].X, Is.EqualTo(Assets[i].X).Within(0.001));
                Assert.That(layouts[0].Assets[i].Y, Is.EqualTo(Assets[i].Y).Within(0.001));
                Assert.That(layouts[0].Assets[i].Z, Is.EqualTo(Assets[i].Z).Within(0.001));
            }
        }
    }
}
