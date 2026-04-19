using System;

namespace Applications.Features.Documents.Dto;

public sealed class DocumentDto
{
    public Guid Id { get; set; }

    public string? OriginalName { get; set; }

    public string? NewFileName { get; set; }

    public string? FileUrl { get; set; }

    public long? FileSize { get; set; }

    public string? AltText { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset? Created { get; set; }
}
