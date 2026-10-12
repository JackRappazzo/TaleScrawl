using System;
using System.Collections.Generic;
using System.Linq;
using TaleScrawl.Core.Ds;
using TaleScrawl.Core.Geometry;
using TaleScrawl.Core.Options;
using TaleScrawl.Core.Slab;

namespace TaleScrawl.Core;

public sealed class TaleScrawlConverter
{
    private readonly SlabOptions _options;
    private readonly DsFileReader _reader;
    private readonly FloorRasterizer _rasterizer;
    private readonly SlabEncoder _encoder;

    public TaleScrawlConverter()
        : this(new SlabOptions())
    {
    }

    public TaleScrawlConverter(SlabOptions options)
    {
        _options = options;
        _reader = new DsFileReader();
        _rasterizer = new FloorRasterizer();
        _encoder = new SlabEncoder();
    }

    public string Convert(string dsFilePath)
    {
        DsDocument document = _reader.Read(dsFilePath);
        return Convert(document);
    }

    public string Convert(DsDocument document)
    {
        double cellDiameter = ReadCellDiameter(document);
        IReadOnlyList<Polygon> polygons = ExtractPolygons(document, cellDiameter);
        HashSet<(int X, int Y)> cells = _rasterizer.Rasterize(polygons);
        IReadOnlyList<AssetPlacement> placements = BuildPlacements(cells);

        string slab = _encoder.Encode(_options.FloorGuid, placements);
        return _options.WrapInBackticks ? "```" + slab + "```" : slab;
    }

    private static double ReadCellDiameter(DsDocument document)
    {
        Dictionary<string, DsNode>? nodes = document.State?.Document?.Nodes;
        if (nodes == null)
        {
            return 1.0;
        }

        if (TryGetDocumentNode(nodes, out DsNode? documentNode) &&
            documentNode!.Children is { Count: > 0 })
        {
            string firstChildId = documentNode.Children[0];
            if (nodes.TryGetValue(firstChildId, out DsNode? pageNode) &&
                pageNode.Grid != null && pageNode.Grid.CellDiameter > 0)
            {
                return pageNode.Grid.CellDiameter;
            }
        }

        foreach (DsNode node in nodes.Values)
        {
            if (node.Grid != null && node.Grid.CellDiameter > 0)
            {
                return node.Grid.CellDiameter;
            }
        }

        return 1.0;
    }

    private static bool TryGetDocumentNode(Dictionary<string, DsNode> nodes, out DsNode? documentNode)
    {
        if (nodes.TryGetValue("document", out documentNode) && documentNode.Type == "DOCUMENT")
        {
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

        foreach (DsGeometry geometryEntry in geometry.Values)
        {
            if (geometryEntry.Polygons == null)
            {
                continue;
            }

            foreach (List<List<Point>> polygonContours in geometryEntry.Polygons)
            {
                var contours = polygonContours
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
        foreach (Point point in contour)
        {
            scaled.Add(new Point(point.X / cellDiameter, point.Y / cellDiameter));
        }

        return scaled;
    }

    private IReadOnlyList<AssetPlacement> BuildPlacements(IReadOnlyCollection<(int X, int Y)> cells)
    {
        if (cells.Count == 0)
        {
            return Array.Empty<AssetPlacement>();
        }

        int minX = cells.Min(c => c.X);
        int minY = cells.Min(c => c.Y);
        int maxX = cells.Max(c => c.X);

        var placements = new List<AssetPlacement>(cells.Count);
        foreach ((int x, int y) in cells)
        {
            double tileX = _options.FlipX ? (double)(maxX - x) : (double)(x - minX);
            double tileZ = (double)(y - minY);

            placements.Add(new AssetPlacement(
                tileX * _options.Scale,
                _options.FloorHeight,
                tileZ * _options.Scale));
        }

        return placements;
    }
}
