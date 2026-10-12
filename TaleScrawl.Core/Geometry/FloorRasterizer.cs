using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleScrawl.Core.Geometry;

public sealed class FloorRasterizer
{
    public HashSet<(int X, int Y)> Rasterize(IEnumerable<Polygon> polygons)
    {
        var cells = new HashSet<(int, int)>();

        foreach (Polygon polygon in polygons)
        {
            RasterizePolygon(polygon, cells);
        }

        return cells;
    }

    private static void RasterizePolygon(Polygon polygon, HashSet<(int, int)> cells)
    {
        if (polygon.Contours.Count == 0)
        {
            return;
        }

        IReadOnlyList<Point> outer = polygon.Contours[0];
        if (outer.Count == 0)
        {
            return;
        }

        var allPoints = polygon.Contours.SelectMany(c => c).ToList();
        if (allPoints.Count == 0)
        {
            return;
        }

        double minX = allPoints.Min(p => p.X);
        double maxX = allPoints.Max(p => p.X);
        double minY = allPoints.Min(p => p.Y);
        double maxY = allPoints.Max(p => p.Y);

        int x0 = (int)Math.Floor(minX);
        int x1 = (int)Math.Ceiling(maxX);
        int y0 = (int)Math.Floor(minY);
        int y1 = (int)Math.Ceiling(maxY);

        for (int cx = x0; cx < x1; cx++)
        {
            for (int cy = y0; cy < y1; cy++)
            {
                var center = new Point(cx + 0.5, cy + 0.5);
                if (IsInside(center, polygon.Contours))
                {
                    cells.Add((cx, cy));
                }
            }
        }
    }

    private static bool IsInside(Point point, IReadOnlyList<IReadOnlyList<Point>> contours)
    {
        int crossings = 0;

        foreach (IReadOnlyList<Point> contour in contours)
        {
            crossings += CrossingCount(point, contour);
        }

        return (crossings & 1) == 1;
    }

    private static int CrossingCount(Point point, IReadOnlyList<Point> contour)
    {
        int count = 0;
        int n = contour.Count;

        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            Point pi = contour[i];
            Point pj = contour[j];

            if ((pi.Y > point.Y) != (pj.Y > point.Y) &&
                point.X < (pj.X - pi.X) * (point.Y - pi.Y) / (pj.Y - pi.Y) + pi.X)
            {
                count++;
            }
        }

        return count;
    }
}
