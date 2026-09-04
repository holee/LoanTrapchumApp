using Application.Interfaces;
using Application.Utils;

namespace Application.Features.Documents.Command.DeleteDocument;

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
