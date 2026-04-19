using Applications.Features.Documents.Dto;
using ErrorOr;

namespace Applications.Features.Documents.Command.AddDocument;

public sealed record AddDocumentCommand(DocumentDtoCreate Document) : IRequest<ErrorOr<DocumentDto>>;
