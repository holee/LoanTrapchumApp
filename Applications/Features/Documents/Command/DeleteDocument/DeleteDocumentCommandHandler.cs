using Applications.Interfaces;
using Applications.Utils;

namespace Applications.Features.Documents.Command.DeleteDocument;

public sealed class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, bool>
{
    private readonly IDocumentServices _documentServices;

    public DeleteDocumentCommandHandler(IDocumentServices documentServices)
    {
        _documentServices = documentServices;
    }

    public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken = default)
    {
        return await _documentServices.DeleteDocumentAsync(request.DocumentId, cancellationToken);
    }
}
