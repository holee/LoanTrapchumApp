using Applications.Utils;
using Domains.Models;

namespace Applications.Features.Documents.Queries.GetDocuments;

public sealed record GetDocumentsQuery() : IRequest<IEnumerable<LtDocument>>;
