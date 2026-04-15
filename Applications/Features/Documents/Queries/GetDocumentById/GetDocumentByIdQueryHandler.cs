using Applications.Interfaces;
using Applications.Utils;
using Domains.Models;

namespace Applications.Features.Documents.Queries.GetDocumentById;

public sealed class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, LtDocument?>
{
    private readonly IDocumentServices _documentServices;

    public GetDocumentByIdQueryHandler(IDocumentServices documentServices)
    {
        _documentServices = documentServices;
    }

    public async Task<LtDocument?> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken = default)
    {
        return await _documentServices.GetDocumentByIdAsync(request.DocumentId, cancellationToken);
    }
}
