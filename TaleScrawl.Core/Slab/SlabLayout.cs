using System;
using System.Collections.Generic;

namespace TaleScrawl.Core.Slab;

public sealed class SlabLayout
{
    public Guid AssetKindId { get; }

    public IReadOnlyList<AssetPlacement> Assets { get; }

    public SlabLayout(Guid assetKindId, IReadOnlyList<AssetPlacement> assets)
    {
        AssetKindId = assetKindId;
        Assets = assets;
    }
}
