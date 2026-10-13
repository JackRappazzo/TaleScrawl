using System.Collections.Generic;
using System.Linq;

namespace TaleScrawl.Core.Geometry;

public sealed class WallRasterizer
{
    // Quarter turns (0-3). The direction a wall faces, toward the floor.
    // These are a first guess at the asset's orientation: verify in-game.
    // If north/south look mirrored while east/west are right, swap FacesSouth and FacesNorth.
    private static class Facing
    {
        public const byte East = 0;
        public const byte South = 1;
        public const byte West = 2;
        public const byte North = 3;
    }

    // Offset from a floor cell to the empty cell its wall sits in (DungeonScrawl grid space).
    private readonly record struct Edge(int DX, int DY, byte QuarterTurns);

    private static readonly Edge[] Edges =
    {
        new(-1,  0, Facing.East),    // wall west of floor, facing east toward the floor
        new( 1,  0, Facing.West),    // wall east of floor, facing west
        new( 0, -1, Facing.South),   // wall north of floor, facing south
        new( 0,  1, Facing.North),   // wall south of floor, facing north
    };

    // Offset from a floor cell to the empty cell its corner piece sits in (DungeonScrawl grid space).
    // QuarterTurns assumes the corner piece at 0 has the floor to its south-east, and each
    // further quarter turn goes clockwise (SE -> SW -> NW -> NE), matching the wall facings above.
    // A first guess: verify in-game. If every corner is off by the same amount, shift the four values.
    private static readonly Edge[] CornerOffsets =
    {
        new(-1, -1, 1),   // corner NW of floor
        new( 1, -1, 2),   // corner NE of floor
        new( 1,  1, 3),   // corner SE of floor
        new(-1,  1, 0),   // corner SW of floor
    };

    /// <summary>
    /// One wall per (floor cell, open edge) pair. A cell open on two sides
    /// (concave corners, one-cell gaps) correctly receives two walls.
    /// </summary>
    public IReadOnlyList<TilePlacement> RasterizeWalls(IReadOnlySet<(int X, int Y)> floor)
    {
        var walls = new List<TilePlacement>();

        foreach ((int x, int y) in floor)
        {
            foreach (Edge e in Edges)
            {
                if (!floor.Contains((x + e.DX, y + e.DY)))
                {
                    walls.Add(new TilePlacement(x + e.DX, y + e.DY, e.QuarterTurns));
                }
            }
        }

        return TilePlacements.Sorted(walls);
    }

    /// <summary>
    /// One corner per (floor cell, diagonal) pair where the corner cell touches no floor
    /// orthogonally. Rotation comes from the diagonal, so a cell diagonal to floor on two
    /// sides correctly receives two differently-rotated corners.
    /// </summary>
    public IReadOnlyList<TilePlacement> RasterizeCorners(IReadOnlySet<(int X, int Y)> floor)
    {
        var corners = new List<TilePlacement>();

        foreach ((int x, int y) in floor)
        {
            foreach (Edge d in CornerOffsets)
            {
                var cell = (X: x + d.DX, Y: y + d.DY);
                if (floor.Contains(cell) || TouchesFloorOrthogonally(cell, floor))
                {
                    continue;
                }

                corners.Add(new TilePlacement(cell.X, cell.Y, d.QuarterTurns));
            }
        }

        return TilePlacements.Sorted(corners);
    }

    private static bool TouchesFloorOrthogonally((int X, int Y) cell, IReadOnlySet<(int X, int Y)> floor)
        => Edges.Any(e => floor.Contains((cell.X + e.DX, cell.Y + e.DY)));
}