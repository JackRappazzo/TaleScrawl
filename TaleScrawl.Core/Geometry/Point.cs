using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaleScrawl.Core.Geometry;

public readonly record struct Point(double X, double Y);

public sealed class PointJsonConverter : JsonConverter<Point>
{
    public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected a two-element array when reading a Point.");
        }

        double x = 0;
        double y = 0;
        int index = 0;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                double value = reader.GetDouble();
                if (index == 0)
                {
                    x = value;
                }
                else if (index == 1)
                {
                    y = value;
                }

                index++;
            }
        }

        return new Point(x, y);
    }

    public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteEndArray();
    }
}
