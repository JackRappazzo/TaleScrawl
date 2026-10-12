using System;
using System.Globalization;
using System.IO;
using TaleScrawl.Core;
using TaleScrawl.Core.Options;
using TextCopy;

return await Run(args);

static async Task<int> Run(string[] args)
{
    if (args.Length == 0)
    {
        PrintUsage();
        return 1;
    }

    string input = args[0];
    if (!File.Exists(input))
    {
        Console.Error.WriteLine($"File not found: {input}");
        return 1;
    }

    var options = new SlabOptions();
    string? output = null;
    bool noClipboard = false;

    for (int i = 1; i < args.Length; i++)
    {
        switch (args[i])
        {
            case "--output" when i + 1 < args.Length:
                output = args[++i];
                break;
            case "--floor-guid" when i + 1 < args.Length:
                options.FloorGuid = Guid.Parse(args[++i]);
                break;
            case "--scale" when i + 1 < args.Length:
                options.Scale = double.Parse(args[++i], CultureInfo.InvariantCulture);
                break;
            case "--height" when i + 1 < args.Length:
                options.FloorHeight = double.Parse(args[++i], CultureInfo.InvariantCulture);
                break;
            case "--flip-x":
                options.FlipX = true;
                break;
            case "--wrap":
                options.WrapInBackticks = true;
                break;
            case "--no-clipboard":
                noClipboard = true;
                break;
            default:
                Console.Error.WriteLine($"Unknown argument: {args[i]}");
                return 1;
        }
    }

    try
    {
        var converter = new TaleScrawlConverter(options);
        string slab = converter.Convert(input);

        if (output != null)
        {
            File.WriteAllText(output, slab);
            Console.Error.WriteLine($"Wrote slab ({slab.Length} chars) to {output}");
        }

        if (!noClipboard)
        {
            await ClipboardService.SetTextAsync(slab);
            Console.Error.WriteLine($"Copied slab ({slab.Length} chars) to clipboard.");
        }
        else if (output == null)
        {
            Console.WriteLine(slab);
        }

        return 0;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error: {ex.Message}");
        return 1;
    }
}

static void PrintUsage()
{
    Console.Error.WriteLine("Usage: TaleScrawl <input.ds> [options]");
    Console.Error.WriteLine();
    Console.Error.WriteLine("Options:");
    Console.Error.WriteLine("  --output <path>       Write slab code to a file");
    Console.Error.WriteLine("  --no-clipboard        Do not copy the result to the clipboard");
    Console.Error.WriteLine("  --floor-guid <guid>   Override the floor tile GUID");
    Console.Error.WriteLine("  --scale <value>       Tile units per grid cell (default 1)");
    Console.Error.WriteLine("  --height <value>      Floor Y height (default 0)");
    Console.Error.WriteLine("  --flip-x              Mirror the X axis");
    Console.Error.WriteLine("  --wrap                Wrap output in triple backticks");
}
