using Application.Features.Documents.Dto;
using Application.Utils;
using Domains.Models;

namespace Application.Features.Documents.Queries.GetDocuments;

public sealed record GetDocumentsQuery() : IRequest<IEnumerable<DocumentDto>>;
