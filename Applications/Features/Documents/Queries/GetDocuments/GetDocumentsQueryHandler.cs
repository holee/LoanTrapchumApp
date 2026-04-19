using Applications.Commons.Mappers;
using Applications.Features.Documents.Dto;
using Applications.Interfaces;
using Applications.Utils;
using Domains.Models;

namespace Applications.Features.Documents.Queries.GetDocuments;

public sealed class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, IEnumerable<DocumentDto>>
{
    private readonly IDocumentServices _documentServices;

    public GetDocumentsQueryHandler(IDocumentServices documentServices)
    {
        _documentServices = documentServices;
    }

    public async Task<IEnumerable<DocumentDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken = default)
    {
        var rsp = await _documentServices.GetDocumentsAsync(null, cancellationToken);

        return rsp.Select(d => d.ToDocumentDto());

    }
}
