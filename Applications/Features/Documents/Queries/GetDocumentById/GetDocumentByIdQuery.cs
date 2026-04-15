using Applications.Utils;
using Domains.Models;

namespace Applications.Features.Documents.Queries.GetDocumentById;

public sealed record GetDocumentByIdQuery(Guid DocumentId) : IRequest<LtDocument?>;
