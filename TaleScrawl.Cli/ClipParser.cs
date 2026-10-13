using System;
using System.Globalization;
using TaleScrawl.Core.Options;

namespace TaleScrawl.Cli;

public sealed record CliArguments(
    string InputPath,
    string? OutputPath,
    bool CopyToClipboard,
    SlabOptions Slab);

/// <summary>Exactly one of Arguments, Error, or ShowHelp is meaningful.</summary>
public sealed record ParseResult(CliArguments? Arguments, string? Error, bool ShowHelp)
{
    public static ParseResult Ok(CliArguments args) => new(args, null, false);
    public static ParseResult Fail(string error) => new(null, error, false);
    public static ParseResult Help() => new(null, null, true);
}

public static class CliParser
{
    public static ParseResult Parse(string[] args)
    {
        if (args.Length == 0)
        {
            return ParseResult.Fail("No input file specified.");
        }

        string? input = null;
        string? output = null;
        bool copyToClipboard = true;
        var options = new SlabOptions();

        try
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                switch (arg)
                {
                    case "-h":
                    case "--help":
                        return ParseResult.Help();

                    case "--output":
                        output = Value(args, ref i);
                        break;
                    case "--floor-guid":
                        options.FloorGuid = ParseGuid(args, ref i);
                        break;
                    case "--wall-guid":
                        options.WallGuid = ParseGuid(args, ref i);
                        break;
                    case "--corner-guid":
                        options.CornerGuid = ParseGuid(args, ref i);
                        break;
                    case "--scale":
                        options.Scale = ParsePositive(args, ref i);
                        break;
                    case "--height":
                        options.FloorHeight = ParseFinite(args, ref i);
                        break;
                    case "--floor-only":
                        options.IncludeWalls = false;
                        break;
                    case "--wrap":
                        options.WrapInBackticks = true;
                        break;
                    case "--no-clipboard":
                        copyToClipboard = false;
                        break;

                    default:
                        if (arg.StartsWith('-'))
                        {
                            return ParseResult.Fail($"Unknown option: {arg}");
                        }

                        if (input != null)
                        {
                            return ParseResult.Fail($"Unexpected extra argument: {arg}");
                        }

                        input = arg;
                        break;
                }
            }
        }
        catch (ArgumentException ex)
        {
            return ParseResult.Fail(ex.Message);
        }

        return input == null
            ? ParseResult.Fail("No input file specified.")
            : ParseResult.Ok(new CliArguments(input, output, copyToClipboard, options));
    }

    // Each helper consumes the value after the option at args[i] and advances i.
    private static string Value(string[] args, ref int i)
    {
        string name = args[i];
        if (i + 1 >= args.Length)
        {
            throw new ArgumentException($"{name} requires a value.");
        }

        return args[++i];
    }

    private static Guid ParseGuid(string[] args, ref int i)
    {
        string name = args[i];
        string text = Value(args, ref i);
        return Guid.TryParse(text, out Guid guid)
            ? guid
            : throw new ArgumentException($"{name}: '{text}' is not a valid GUID.");
    }

    private static double ParseFinite(string[] args, ref int i)
    {
        string name = args[i];
        string text = Value(args, ref i);

        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value) ||
            !double.IsFinite(value))
        {
            throw new ArgumentException($"{name}: '{text}' is not a valid number.");
        }

        return value;
    }

    private static double ParsePositive(string[] args, ref int i)
    {
        string name = args[i];
        double value = ParseFinite(args, ref i);
        return value > 0
            ? value
            : throw new ArgumentException($"{name} must be greater than zero.");
    }
}