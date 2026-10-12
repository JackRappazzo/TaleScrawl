namespace TaleScrawl.Core.Slab;

public readonly record struct AssetPlacement(
    double X,
    double Y,
    double Z,
    byte Rotation = 0,
    byte ExtraBits = 0);
