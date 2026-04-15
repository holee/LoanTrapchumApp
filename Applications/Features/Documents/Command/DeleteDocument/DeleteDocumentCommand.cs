using Applications.Utils;

namespace Applications.Features.Documents.Command.DeleteDocument;

public sealed record DeleteDocumentCommand(Guid DocumentId) : IRequest<bool>;
