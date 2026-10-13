using System;
using System.Collections.Generic;
using TaleScrawl.Core.Geometry;
using TaleScrawl.Core.Options;
using TaleSpire.Slab;
using TaleSpire.Slab.V2;

namespace TaleScrawl.Core;

/// <summary>
/// Maps DungeonScrawl grid cells (Y up) to TaleSpire slab positions (Z, flipped),
/// anchored so the slab's minimum corner sits at the origin.
/// </summary>
internal readonly record struct GridTransform(int MinX, int MaxY, double Scale, double Height)
{
    // TaleSpire stores rotation as 0-15 (16 positions per turn); tiles may only use
    // multiples of 4. So one quarter turn = 4 raw steps = 90 degrees.
    // ASSUMPTION: AssetData's float rotation is in degrees. If its docs say it takes the
    // raw 0-15 value instead, use tile.QuarterTurns * 4f here.
    private const float DegreesPerQuarterTurn = 90f;

    public static GridTransform Fit(IEnumerable<TilePlacement> tiles, SlabOptions options)
    {
        int minX = int.MaxValue;
        int maxY = int.MinValue;
        bool any = false;

        foreach (TilePlacement t in tiles)
        {
            any = true;
            if (t.X < minX) minX = t.X;
            if (t.Y > maxY) maxY = t.Y;
        }

        if (!any)
        {
            throw new InvalidOperationException("Cannot fit a transform to zero tiles.");
        }

        return new GridTransform(minX, maxY, options.Scale, options.FloorHeight);
    }

    public AssetData ToAsset(TilePlacement tile)
    {
        var position = new Float3(
            (float)((tile.X - MinX) * Scale),
            (float)Height,
            (float)((MaxY - tile.Y) * Scale));

        return new AssetData(position, tile.QuarterTurns * DegreesPerQuarterTurn, new ExtraBits(0));
    }
}