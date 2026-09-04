using Application.Features.Documents.Dto;
using Domains.Models;

namespace Application.Commons.Mappers.Documents
{
    public static class DocumentMapper
    {
        public static DocumentDto ToDto(this LtDocument? source)
        {
            if (source is null) return null!;

            return new DocumentDto
            {
                Id = source.Id,
                OriginalName = source.OriginalName,
                NewFileName = source.NewFileName,
                FileUrl = source.FileUrl,
                FileSize = source.FileSize,
                AltText = source.AltText,
                Description = source.Description,
                Created = source.Created
            };
        }

        public static LtDocument ToEntity(this DocumentDto? source)
        {
            if (source is null) return null!;

            return new LtDocument
            {
                Id = source.Id == Guid.Empty ? Guid.NewGuid() : source.Id,
                OriginalName = source.OriginalName,
                NewFileName = source.NewFileName,
                FileUrl = source.FileUrl,
                FileSize = source.FileSize,
                AltText = source.AltText,
                Description = source.Description,
                Created = source.Created ?? DateTimeOffset.UtcNow
            };
        }
    }
}
