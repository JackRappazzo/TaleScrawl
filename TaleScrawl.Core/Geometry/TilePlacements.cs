using System.Collections.Generic;
using System.Linq;

namespace TaleScrawl.Core.Geometry;

/// <summary>
/// One tile on the grid, in DungeonScrawl grid space (Y not yet flipped).
/// QuarterTurns is 0-3 (0°, 90°, 180°, 270°). Tiles in TaleSpire only allow
/// these four rotations, so the domain model never deals in TaleSpire's raw units.
/// </summary>
public readonly record struct TilePlacement(int X, int Y, byte QuarterTurns = 0);

public static class TilePlacements
{
    /// <summary>Stable ordering so the same input always yields the same slab string.</summary>
    public static IReadOnlyList<TilePlacement> Sorted(IEnumerable<TilePlacement> tiles)
        => tiles.OrderBy(t => t.Y).ThenBy(t => t.X).ThenBy(t => t.QuarterTurns).ToList();
}