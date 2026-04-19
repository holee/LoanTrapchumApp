using Applications.Features.Documents.Dto;
using Domains.Models;

namespace Applications.Commons.Mappers;

public static class DocumentMapperExtensions
{

    extension(DocumentDtoCreate ex)
    {
        public LtDocument ToDocument()
        {
            return new LtDocument
            {
                OriginalName = ex.OriginalName,
                NewFileName = ex.NewFileName,
                FileUrl = ex.FileUrl,
                FileSize = ex.FileSize,
                AltText = ex.AltText,
                Description = ex.Description
            };
        }
    }

    extension(DocumentDtoUpdate ex)
    {
        public LtDocument ToDocument() 
        {
            return new LtDocument
            {
                OriginalName = ex.OriginalName,
                NewFileName = ex.NewFileName,
                FileUrl = ex.FileUrl,
                FileSize = ex.FileSize,
                AltText = ex.AltText,
                Description = ex.Description
            };
        }
    }


    extension(LtDocument ex)
    {
        public DocumentDto ToDocumentDto()
        {
            return new DocumentDto
            {
                Id = ex.Id,
                OriginalName = ex.OriginalName,
                NewFileName = ex.NewFileName,
                FileUrl = ex.FileUrl,
                FileSize = ex.FileSize,
                AltText = ex.AltText,
                Description = ex.Description,
            };
        }

        public DocumentDtoUpdate ToDocumentDtoUpdate()
        {
            return new DocumentDtoUpdate
            {
                Id = ex.Id,
                OriginalName = ex.OriginalName,
                NewFileName = ex.NewFileName,
                FileUrl = ex.FileUrl,
                FileSize = ex.FileSize,
                AltText = ex.AltText,
                Description = ex.Description,
            };
        }
    }


}