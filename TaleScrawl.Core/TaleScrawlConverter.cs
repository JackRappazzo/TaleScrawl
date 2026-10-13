using System;
using System.Collections.Generic;
using System.Linq;
using TaleScrawl.Core.DungeonScrawl;
using TaleScrawl.Core.Geometry;
using TaleScrawl.Core.Options;
using TaleSpire.Slab;
using TaleSpire.Slab.V2;

namespace TaleScrawl.Core;

public sealed class TaleScrawlConverter
{
    private readonly SlabOptions _options;
    private readonly DsFileReader _reader;
    private readonly FloorRasterizer _floorRasterizer;
    private readonly WallRasterizer _wallRasterizer;

    public TaleScrawlConverter()
        : this(options: null, reader: null, floorRasterizer: null, wallRasterizer: null)
    {
    }

    public TaleScrawlConverter(
        SlabOptions? options = null,
        DsFileReader? reader = null,
        FloorRasterizer? floorRasterizer = null,
        WallRasterizer? wallRasterizer = null)
    {
        _options = options ?? new SlabOptions();
        _reader = reader ?? new DsFileReader();
        _floorRasterizer = floorRasterizer ?? new FloorRasterizer();
        _wallRasterizer = wallRasterizer ?? new WallRasterizer();
    }

    public string Convert(string dsFilePath)
        => Convert(_reader.Read(dsFilePath));

    public string Convert(DsDocument document)
        => Slabs.Export(BuildSlab(document), _options.WrapInBackticks);

    /// <summary>Builds the slab without encoding it, so tests can inspect it directly.</summary>
    public Slab BuildSlab(DsDocument document)
    {
        MapModel map = DsMapMapper.Map(document);
        HashSet<(int X, int Y)> floor = _floorRasterizer.Rasterize(map.Polygons);

        if (floor.Count == 0)
        {
            throw new InvalidOperationException("The map contains no floor cells.");
        }

        IReadOnlyList<TileLayer> layers = BuildLayers(floor);
        GridTransform transform = GridTransform.Fit(layers.SelectMany(l => l.Tiles), _options);

        return new Slab(layers.Select(l => BuildLayout(l, transform)).ToList());
    }

    private IReadOnlyList<TileLayer> BuildLayers(HashSet<(int X, int Y)> floor)
    {
        var layers = new List<TileLayer>
        {
            new(_options.FloorGuid,
                TilePlacements.Sorted(floor.Select(c => new TilePlacement(c.X, c.Y))))
        };

        if (_options.IncludeWalls)
        {
            if (_options.WallGuid != Guid.Empty)
            {
                layers.Add(new TileLayer(_options.WallGuid, _wallRasterizer.RasterizeWalls(floor)));
            }

            if (_options.CornerGuid != Guid.Empty)
            {
                layers.Add(new TileLayer(_options.CornerGuid, _wallRasterizer.RasterizeCorners(floor)));
            }
        }

        return layers;
    }

    private static Layout BuildLayout(TileLayer layer, GridTransform transform)
    {
        if (layer.Tiles.Count > ushort.MaxValue)
        {
            throw new InvalidOperationException(
                $"Layer {layer.Guid} has {layer.Tiles.Count} tiles; a slab layout holds at most {ushort.MaxValue}.");
        }

        var layout = new Layout(new LayoutData(layer.Guid, (ushort)layer.Tiles.Count));
        for (int i = 0; i < layer.Tiles.Count; i++)
        {
            layout.Assets[i] = transform.ToAsset(layer.Tiles[i]);
        }

        return layout;
    }

    private sealed record TileLayer(Guid Guid, IReadOnlyList<TilePlacement> Tiles);
}