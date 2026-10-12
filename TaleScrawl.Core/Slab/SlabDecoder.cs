using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TaleScrawl.Core.Slab;

public sealed class SlabDecoder
{
    private const uint MagicHex = 0xD1CEFACE;
    private const ushort CurrentVersion = 2;
    private const int PositionBits = 18;
    private const int PositionMask = (1 << PositionBits) - 1;

    public IReadOnlyList<SlabLayout> Decode(string slab)
    {
        string trimmed = slab.Trim();

        if (trimmed.StartsWith("```", StringComparison.Ordinal))
        {
            trimmed = trimmed.Substring(3);
            int endIndex = trimmed.LastIndexOf("```", StringComparison.Ordinal);
            if (endIndex >= 0)
            {
                trimmed = trimmed.Substring(0, endIndex);
            }

            trimmed = trimmed.Trim();
        }

        byte[] compressed = Convert.FromBase64String(trimmed);
        byte[] binary = Decompress(compressed);
        return Parse(binary);
    }

    private static byte[] Decompress(byte[] data)
    {
        using var input = new MemoryStream(data);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);
        return output.ToArray();
    }

    private static IReadOnlyList<SlabLayout> Parse(byte[] data)
    {
        using var memory = new MemoryStream(data);
        using var reader = new BinaryReader(memory);

        uint magic = reader.ReadUInt32();
        if (magic != MagicHex)
        {
            throw new InvalidDataException("Invalid slab magic value.");
        }

        ushort version = reader.ReadUInt16();
        if (version != CurrentVersion)
        {
            throw new InvalidDataException($"Unsupported slab version: {version}.");
        }

        ushort layoutCount = reader.ReadUInt16();
        ushort creatureCount = reader.ReadUInt16();
        if (creatureCount != 0)
        {
            throw new InvalidDataException("Unexpected creature count in slab.");
        }

        var layouts = new List<SlabLayout>(layoutCount);
        var assetCounts = new ushort[layoutCount];

        for (int i = 0; i < layoutCount; i++)
        {
            byte[] guidBytes = reader.ReadBytes(16);
            var guid = new Guid(guidBytes);
            ushort assetCount = reader.ReadUInt16();
            reader.ReadUInt16();

            assetCounts[i] = assetCount;
            layouts.Add(new SlabLayout(guid, new List<AssetPlacement>(assetCount)));
        }

        for (int i = 0; i < layoutCount; i++)
        {
            var assets = (List<AssetPlacement>)layouts[i].Assets;
            for (int j = 0; j < assetCounts[i]; j++)
            {
                assets.Add(UnpackAsset(reader.ReadUInt64()));
            }
        }

        return layouts;
    }

    private static AssetPlacement UnpackAsset(ulong encoded)
    {
        int scaledX = (int)(encoded & PositionMask);
        int scaledY = (int)((encoded >> 18) & PositionMask);
        int scaledZ = (int)((encoded >> 36) & PositionMask);
        byte rotation = (byte)((encoded >> 54) & 0x1F);
        byte extraBits = (byte)((encoded >> 59) & 0x1F);

        return new AssetPlacement(
            scaledX / 100.0,
            scaledY / 100.0,
            scaledZ / 100.0,
            rotation,
            extraBits);
    }
}
