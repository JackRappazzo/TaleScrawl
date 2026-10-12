using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TaleScrawl.Core.Slab;

public sealed class SlabEncoder
{
    private const uint MagicHex = 0xD1CEFACE;
    private const ushort CurrentVersion = 2;
    private const int PositionBits = 18;
    private const int PositionMask = (1 << PositionBits) - 1;
    private const int RotationBitsOffset = 54;
    private const int ExtraBitsOffset = 59;

    public string Encode(Guid assetKindId, IReadOnlyList<AssetPlacement> assets)
    {
        var layouts = new List<SlabLayout>
        {
            new(assetKindId, assets)
        };

        return Encode(layouts);
    }

    public string Encode(IReadOnlyList<SlabLayout> layouts)
    {
        byte[] binary = BuildBinary(layouts);
        byte[] compressed = Compress(binary);
        return Convert.ToBase64String(compressed);
    }

    private static byte[] BuildBinary(IReadOnlyList<SlabLayout> layouts)
    {
        using var memory = new MemoryStream();
        using var writer = new BinaryWriter(memory);

        writer.Write(MagicHex);
        writer.Write(CurrentVersion);
        writer.Write((ushort)layouts.Count);
        writer.Write((ushort)0);

        foreach (SlabLayout layout in layouts)
        {
            writer.Write(layout.AssetKindId.ToByteArray());
            writer.Write((ushort)layout.Assets.Count);
            writer.Write((ushort)0);
        }

        foreach (SlabLayout layout in layouts)
        {
            foreach (AssetPlacement asset in layout.Assets)
            {
                writer.Write(PackAsset(asset));
            }
        }

        writer.Flush();
        return memory.ToArray();
    }

    private static ulong PackAsset(AssetPlacement asset)
    {
        int scaledX = (int)Math.Round(asset.X * 100.0);
        int scaledY = (int)Math.Round(asset.Y * 100.0);
        int scaledZ = (int)Math.Round(asset.Z * 100.0);

        if (scaledX < 0 || scaledY < 0 || scaledZ < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(asset), "Asset positions must be greater than or equal to zero.");
        }

        if (scaledX > PositionMask || scaledY > PositionMask || scaledZ > PositionMask)
        {
            throw new ArgumentOutOfRangeException(nameof(asset), "Asset position exceeds the encodable range.");
        }

        ulong maskedX = (ulong)(scaledX & PositionMask);
        ulong maskedY = (ulong)(scaledY & PositionMask);
        ulong maskedZ = (ulong)(scaledZ & PositionMask);
        ulong rotation = (ulong)(asset.Rotation & 0x1F);
        ulong extraBits = (ulong)(asset.ExtraBits & 0x1F);

        return maskedX
            | (maskedY << 18)
            | (maskedZ << 36)
            | (rotation << RotationBitsOffset)
            | (extraBits << ExtraBitsOffset);
    }

    private static byte[] Compress(byte[] data)
    {
        using var memory = new MemoryStream();
        using (var gzip = new GZipStream(memory, CompressionLevel.Optimal, leaveOpen: true))
        {
            gzip.Write(data, 0, data.Length);
        }

        return memory.ToArray();
    }
}
