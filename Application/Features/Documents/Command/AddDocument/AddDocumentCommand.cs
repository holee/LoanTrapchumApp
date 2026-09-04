using Application.Features.Documents.Dto;
using ErrorOr;

namespace Application.Features.Documents.Command.AddDocument;

public sealed record AddDocumentCommand(DocumentDtoCreate Document) : IRequest<ErrorOr<DocumentDto>>;
