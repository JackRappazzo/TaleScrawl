using System;
using System.Collections.Generic;
using TaleScrawl.Core.Slab;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.Slab.EncodeTests
{
    public class WhenEncodeThenArgumentOutOfRangeExceptionIsThrown : WhenTestingEncode
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(AnAssetWithANegativeCoordinateIsProvided)
            .When(EncodeIsCalled)
            .Then(ShouldThrowArgumentOutOfRangeException);

        [Given]
        public void AnAssetWithANegativeCoordinateIsProvided()
        {
            AssetKindId = Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768");
            Assets = new List<AssetPlacement>
            {
                new(-1, 0, 0)
            };
        }

        [Then]
        public void ShouldThrowArgumentOutOfRangeException()
        {
            Assert.That(ThrownException, Is.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}
