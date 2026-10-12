using System;

namespace TaleScrawl.Core.Options;

public sealed class SlabOptions
{
    public Guid FloorGuid { get; set; } = Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768");

    public double Scale { get; set; } = 1.0;

    public double FloorHeight { get; set; } = 0.0;

    public bool FlipX { get; set; }

    public bool WrapInBackticks { get; set; }
}
