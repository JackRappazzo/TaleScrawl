using System;
using TaleScrawl.Cli;

namespace TaleScrawl.UnitTests.Cli.ParseTests
{
    [TestFixture]
    public class CliParserTests
    {
        [Test]
        public void Parse_WithNoArguments_ReturnsError()
        {
            ParseResult result = CliParser.Parse(Array.Empty<string>());

            Assert.That(result.Arguments, Is.Null);
            Assert.That(result.Error, Is.EqualTo("No input file specified."));
            Assert.That(result.ShowHelp, Is.False);
        }

        [Test]
        public void Parse_WithHelpFlag_ShowsHelp()
        {
            ParseResult result = CliParser.Parse(new[] { "--help" });

            Assert.That(result.ShowHelp, Is.True);
            Assert.That(result.Arguments, Is.Null);
            Assert.That(result.Error, Is.Null);
        }

        [Test]
        public void Parse_WithInputFile_SetsDefaults()
        {
            ParseResult result = CliParser.Parse(new[] { "map.ds" });

            Assert.That(result.Arguments, Is.Not.Null);
            Assert.That(result.Arguments!.InputPath, Is.EqualTo("map.ds"));
            Assert.That(result.Arguments.OutputPath, Is.Null);
            Assert.That(result.Arguments.CopyToClipboard, Is.True);
            Assert.That(result.Arguments.Slab.IncludeWalls, Is.True);
        }

        [Test]
        public void Parse_WithFloorOnlyAndOutput_SetsOptions()
        {
            ParseResult result = CliParser.Parse(new[] { "map.ds", "--floor-only", "--output", "out.txt" });

            Assert.That(result.Arguments, Is.Not.Null);
            Assert.That(result.Arguments!.OutputPath, Is.EqualTo("out.txt"));
            Assert.That(result.Arguments.Slab.IncludeWalls, Is.False);
        }

        [Test]
        public void Parse_WithScaleAndHeight_ParsesNumbers()
        {
            ParseResult result = CliParser.Parse(new[] { "map.ds", "--scale", "2.5", "--height", "1" });

            Assert.That(result.Arguments!.Slab.Scale, Is.EqualTo(2.5));
            Assert.That(result.Arguments.Slab.FloorHeight, Is.EqualTo(1.0));
        }

        [Test]
        public void Parse_WithUnknownOption_ReturnsError()
        {
            ParseResult result = CliParser.Parse(new[] { "map.ds", "--bogus" });

            Assert.That(result.Error, Is.EqualTo("Unknown option: --bogus"));
        }

        [Test]
        public void Parse_WithNonPositiveScale_ReturnsError()
        {
            ParseResult result = CliParser.Parse(new[] { "map.ds", "--scale", "0" });

            Assert.That(result.Error, Is.EqualTo("--scale must be greater than zero."));
        }

        [Test]
        public void Parse_WithInvalidGuid_ReturnsError()
        {
            ParseResult result = CliParser.Parse(new[] { "map.ds", "--wall-guid", "nope" });

            Assert.That(result.Error, Is.EqualTo("--wall-guid: 'nope' is not a valid GUID."));
        }

        [Test]
        public void Parse_WithMissingOptionValue_ReturnsError()
        {
            ParseResult result = CliParser.Parse(new[] { "map.ds", "--output" });

            Assert.That(result.Error, Is.EqualTo("--output requires a value."));
        }
    }
}
