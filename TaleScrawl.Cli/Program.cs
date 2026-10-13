using System;
using System.IO;
using System.Threading.Tasks;
using TaleScrawl.Cli;
using TaleScrawl.Core;
using TextCopy;

return await Run(args);

static async Task<int> Run(string[] args)
{
    ParseResult parsed = CliParser.Parse(args);

    if (parsed.ShowHelp)
    {
        PrintUsage(Console.Out);
        return 0;
    }

    if (parsed.Arguments is not { } cli)
    {
        Console.Error.WriteLine($"Error: {parsed.Error}");
        Console.Error.WriteLine();
        PrintUsage(Console.Error);
        return 2;   // usage error
    }

    if (!File.Exists(cli.InputPath))
    {
        Console.Error.WriteLine($"File not found: {cli.InputPath}");
        return 1;
    }

    try
    {
        string slab = new TaleScrawlConverter(cli.Slab).Convert(cli.InputPath);
        return await Deliver(slab, cli);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error: {ex.Message}");
        return 1;
    }
}

// Writes the slab to every requested destination. A clipboard failure is a warning,
// not an error, and stdout is the fallback so the slab is never lost.
static async Task<int> Deliver(string slab, CliArguments cli)
{
    bool delivered = false;

    if (cli.OutputPath != null)
    {
        string? dir = Path.GetDirectoryName(Path.GetFullPath(cli.OutputPath));
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }

        await File.WriteAllTextAsync(cli.OutputPath, slab);
        Console.Error.WriteLine($"Wrote slab ({slab.Length} chars) to {cli.OutputPath}");
        delivered = true;
    }

    if (cli.CopyToClipboard)
    {
        try
        {
            await ClipboardService.SetTextAsync(slab);
            Console.Error.WriteLine($"Copied slab ({slab.Length} chars) to clipboard.");
            delivered = true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Warning: could not copy to clipboard ({ex.Message}).");
        }
    }

    if (!delivered)
    {
        Console.WriteLine(slab);
    }

    return 0;
}

static void PrintUsage(TextWriter w)
{
    w.WriteLine("Usage: TaleScrawl <input.ds> [options]");
    w.WriteLine();
    w.WriteLine("Options:");
    w.WriteLine("  --output <path>       Write slab code to a file");
    w.WriteLine("  --no-clipboard        Do not copy the result to the clipboard");
    w.WriteLine("  --floor-guid <guid>   Override the floor tile GUID");
    w.WriteLine("  --wall-guid <guid>    Override the wall tile GUID");
    w.WriteLine("  --corner-guid <guid>  Override the corner tile GUID");
    w.WriteLine("  --floor-only          Emit floor tiles only (no walls or corners)");
    w.WriteLine("  --scale <value>       Tile units per grid cell (default 1, must be > 0)");
    w.WriteLine("  --height <value>      Floor Y height (default 0)");
    w.WriteLine("  --wrap                Wrap output in triple backticks");
    w.WriteLine("  -h, --help            Show this help");
}