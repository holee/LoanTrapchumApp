using Application.Utils;

namespace Application.Features.Documents.Command.DeleteDocument;

public sealed record DeleteDocumentCommand(Guid DocumentId) : IRequest<bool>;
