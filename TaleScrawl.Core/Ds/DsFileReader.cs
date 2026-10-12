using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using TaleScrawl.Core.Geometry;

namespace TaleScrawl.Core.Ds;

public sealed class DsFileReader
{
    private readonly JsonSerializerOptions _jsonOptions;

    public DsFileReader()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        _jsonOptions.Converters.Add(new PointJsonConverter());
    }

    public DsDocument Read(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        return Read(stream);
    }

    public DsDocument Read(Stream stream)
    {
        string json = ReadJson(stream);
        return JsonSerializer.Deserialize<DsDocument>(json, _jsonOptions)
            ?? throw new InvalidDataException("The .ds file could not be deserialized.");
    }

    private static string ReadJson(Stream stream)
    {
        if (!stream.CanSeek)
        {
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);
            buffer.Position = 0;
            return ReadJson(buffer);
        }

        if (IsZipArchive(stream))
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false);
            ZipArchiveEntry entry = archive.Entries.FirstOrDefault()
                ?? throw new InvalidDataException("The .ds archive contains no entries.");

            using Stream entryStream = entry.Open();
            using var reader = new StreamReader(entryStream);
            return reader.ReadToEnd();
        }

        stream.Position = 0;
        using var plainReader = new StreamReader(stream);
        return plainReader.ReadToEnd();
    }

    private static bool IsZipArchive(Stream stream)
    {
        if (stream.Length < 2)
        {
            return false;
        }

        long originalPosition = stream.Position;

        try
        {
            stream.Position = 0;
            int first = stream.ReadByte();
            int second = stream.ReadByte();
            stream.Position = originalPosition;
            return first == 'P' && second == 'K';
        }
        catch
        {
            stream.Position = originalPosition;
            return false;
        }
    }
}
