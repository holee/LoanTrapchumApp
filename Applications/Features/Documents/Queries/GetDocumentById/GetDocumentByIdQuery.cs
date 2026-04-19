using Applications.Features.Documents.Dto;
using Applications.Utils;
using Domains.Models;
using ErrorOr;

namespace Applications.Features.Documents.Queries.GetDocumentById;

public sealed record GetDocumentByIdQuery(Guid DocumentId) : IRequest<ErrorOr<DocumentDto>>;
