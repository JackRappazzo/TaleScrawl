using System;
using System.Collections.Generic;
using TaleScrawl.Core.Slab;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.Slab.EncodeTests
{
    public abstract class WhenTestingEncode : WhenTestingSlabEncoder
    {
        protected Guid AssetKindId;
        protected List<AssetPlacement> Assets = null!;
        protected string Result = null!;

        [When(DoNotRethrowExceptions: true)]
        public void EncodeIsCalled()
        {
            Result = Encoder.Encode(AssetKindId, Assets);
        }
    }
}
