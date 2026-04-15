using Applications.Interfaces;
using Applications.Utils;
using Domains.Models;

namespace Applications.Features.Documents.Command.AddDocument;

public sealed class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand, LtDocument>
{
    private readonly IDocumentServices _documentServices;

    public AddDocumentCommandHandler(IDocumentServices documentServices)
    {
        _documentServices = documentServices;
    }

    public async Task<LtDocument> Handle(AddDocumentCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _documentServices.AddDocumentAsync(request.Document, cancellationToken);
        return result;
    }
}
