using System;

namespace TaleScrawl.Core.Options;

public sealed class SlabOptions
{
    public Guid FloorGuid { get; set; } = Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768");

    public Guid WallGuid { get; set; } = Guid.Parse("05b8b832-e457-4ea7-88d5-25fa44037f39");

    public Guid CornerGuid { get; set; } = Guid.Parse("ff6b5a27-a041-4ba1-ad33-01d82c77bb34");

    public bool IncludeWalls { get; set; } = true;

    public double Scale { get; set; } = 1.0;

    public double FloorHeight { get; set; } = 0.0;

    public bool WrapInBackticks { get; set; }
}
