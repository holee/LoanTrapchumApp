using Applications.Features.Documents.Dto;
using Applications.Utils;
using Domains.Models;
using ErrorOr;

namespace Applications.Features.Documents.Command.AddDocuments; 

public sealed record AddDocumentsCommand(IEnumerable<DocumentDtoCreate> Documents) : IRequest<IEnumerable<DocumentDto>>;
