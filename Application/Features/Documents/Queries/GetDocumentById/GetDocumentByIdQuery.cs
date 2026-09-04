using Application.Features.Documents.Dto;
using Application.Utils;
using Domains.Models;
using ErrorOr;

namespace Application.Features.Documents.Queries.GetDocumentById;

public sealed record GetDocumentByIdQuery(Guid DocumentId) : IRequest<ErrorOr<DocumentDto>>;
