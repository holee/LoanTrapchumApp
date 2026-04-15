using Applications.Utils;
using Domains.Models;

namespace Applications.Features.Documents.Command.AddDocument;

public sealed record AddDocumentCommand(LtDocument Document) : IRequest<LtDocument>;
