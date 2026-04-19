using Applications.Commons.Mappers;
using Applications.Features.Documents.Dto;
using Applications.Interfaces;
using Applications.Utils;
using Domains.Models;
using ErrorOr;

namespace Applications.Features.Documents.Queries.GetDocumentById;

public sealed class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, ErrorOr<DocumentDto>>
{
    private readonly IDocumentServices _documentServices;

    public GetDocumentByIdQueryHandler(IDocumentServices documentServices)
    {
        _documentServices = documentServices;
    }

    public async Task<ErrorOr<DocumentDto>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken = default)
    {
        var resp = await _documentServices.GetDocumentByIdAsync(request.DocumentId, cancellationToken);

        if(resp == null)  return Error.NotFound("Document not found");

        return resp.ToDocumentDto();
    }
}
