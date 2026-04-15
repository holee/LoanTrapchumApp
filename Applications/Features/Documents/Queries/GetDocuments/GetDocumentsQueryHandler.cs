using Applications.Interfaces;
using Applications.Utils;
using Domains.Models;

namespace Applications.Features.Documents.Queries.GetDocuments;

public sealed class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, IEnumerable<LtDocument>>
{
    private readonly IDocumentServices _documentServices;

    public GetDocumentsQueryHandler(IDocumentServices documentServices)
    {
        _documentServices = documentServices;
    }

    public async Task<IEnumerable<LtDocument>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken = default)
    {
        return await _documentServices.GetDocumentsAsync(null, cancellationToken);
    }
}
