using System.Collections.Generic;
using TaleScrawl.Core.Geometry;

namespace TaleScrawl.Core.Ds;

public sealed class DsDocument
{
    public DsData? Data { get; set; }

    public DsState? State { get; set; }
}

public sealed class DsData
{
    public Dictionary<string, DsGeometry>? Geometry { get; set; }
}

public sealed class DsGeometry
{
    public List<List<List<Point>>>? Polygons { get; set; }

    public List<List<Point>>? Polylines { get; set; }
}

public sealed class DsState
{
    public DsDocumentState? Document { get; set; }
}

public sealed class DsDocumentState
{
    public Dictionary<string, DsNode>? Nodes { get; set; }
}

public sealed class DsNode
{
    public string? Type { get; set; }

    public string? Name { get; set; }

    public DsGrid? Grid { get; set; }

    public List<string>? Children { get; set; }
}

public sealed class DsGrid
{
    public double CellDiameter { get; set; }
}
