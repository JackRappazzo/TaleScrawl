using System.Collections.Generic;

namespace TaleScrawl.Core.Geometry;

public sealed class Polygon
{
    public IReadOnlyList<IReadOnlyList<Point>> Contours { get; }

    public Polygon(IReadOnlyList<IReadOnlyList<Point>> contours)
    {
        Contours = contours;
    }
}
