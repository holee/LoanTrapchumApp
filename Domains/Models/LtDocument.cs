namespace Domains.Models;

using Domains.Models.Lists;
using System;

public sealed class LtDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? OriginalName { get; set; }

    public string? NewFileName { get; set; }

    public string? FileUrl { get; set; }

    public long? FileSize { get; set; }

    public string? AltText { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset? Created { get; set; } = DateTimeOffset.UtcNow;

    public Branch? Branch { get; set; } = null;
}