using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TaleScrawl.Core.Geometry;

namespace TaleScrawl.Core.DungeonScrawl;

/// <summary>Our own view of a DungeonScrawl map: polygons already scaled to grid units.</summary>
public sealed record MapModel(
    IReadOnlyList<Polygon> Polygons,
    double CellDiameter,
    bool UsedDefaultCellDiameter);

/// <summary>
/// The only place that knows the shape of the DungeonScrawl document.
/// Everything downstream works with MapModel.
/// </summary>
public static class DsMapMapper
{
    private const double DefaultCellDiameter = 1.0;
    private const int CoordinateDecimals = 6;   // makes near-ties exact so cell-center sampling is stable

    public static MapModel Map(DsDocument document)
    {
        bool found = TryReadCellDiameter(document, out double diameter);
        if (!found)
        {
            diameter = DefaultCellDiameter;
        }

        return new MapModel(ExtractPolygons(document, diameter), diameter, UsedDefaultCellDiameter: !found);
    }

    private static bool TryReadCellDiameter(DsDocument document, out double diameter)
    {
        diameter = 0;

        Dictionary<string, DsNode>? nodes = document.State?.Document?.Nodes;
        if (nodes == null)
        {
            return false;
        }

        // Prefer the first page under the document node.
        if (TryGetDocumentNode(nodes, out DsNode? documentNode) &&
            documentNode.Children is { Count: > 0 } &&
            nodes.TryGetValue(documentNode.Children[0], out DsNode? pageNode) &&
            pageNode.Grid is { CellDiameter: > 0 } pageGrid)
        {
            diameter = pageGrid.CellDiameter;
            return true;
        }

        // Otherwise any node with a usable grid.
        foreach (DsNode node in nodes.Values)
        {
            if (node.Grid is { CellDiameter: > 0 } grid)
            {
                diameter = grid.CellDiameter;
                return true;
            }
        }

        return false;
    }

    private static bool TryGetDocumentNode(
        Dictionary<string, DsNode> nodes,
        [NotNullWhen(true)] out DsNode? documentNode)
    {
        if (nodes.TryGetValue("document", out DsNode? byKey) && byKey.Type == "DOCUMENT")
        {
            documentNode = byKey;
            return true;
        }

        documentNode = nodes.Values.FirstOrDefault(n => n.Type == "DOCUMENT");
        return documentNode != null;
    }

    private static IReadOnlyList<Polygon> ExtractPolygons(DsDocument document, double cellDiameter)
    {
        var polygons = new List<Polygon>();

        Dictionary<string, DsGeometry>? geometry = document.Data?.Geometry;
        if (geometry == null)
        {
            return polygons;
        }

        foreach (DsGeometry entry in geometry.Values)
        {
            if (entry.Polygons == null)
            {
                continue;
            }

            foreach (List<List<Point>> polygonContours in entry.Polygons)
            {
                List<IReadOnlyList<Point>> contours = polygonContours
                    .Where(c => c.Count > 0)
                    .Select(c => (IReadOnlyList<Point>)ScaleContour(c, cellDiameter))
                    .ToList();

                if (contours.Count > 0)
                {
                    polygons.Add(new Polygon(contours));
                }
            }
        }

        return polygons;
    }

    private static List<Point> ScaleContour(List<Point> contour, double cellDiameter)
    {
        var scaled = new List<Point>(contour.Count);
        foreach (Point p in contour)
        {
            scaled.Add(new Point(
                Math.Round(p.X / cellDiameter, CoordinateDecimals),
                Math.Round(p.Y / cellDiameter, CoordinateDecimals)));
        }

        return scaled;
    }
}