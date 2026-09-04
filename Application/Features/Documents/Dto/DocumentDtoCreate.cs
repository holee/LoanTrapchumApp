namespace Application.Features.Documents.Dto;

public class DocumentDtoCreate
{
    public string? OriginalName { get; set; }

    public string? NewFileName { get; set; }

    public string? FileUrl { get; set; }

    public long? FileSize { get; set; }

    public string? AltText { get; set; }

    public string? Description { get; set; }

}
