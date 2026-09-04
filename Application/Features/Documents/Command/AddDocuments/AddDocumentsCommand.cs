using Application.Features.Documents.Dto;
using Application.Utils;
using Domains.Models;
using ErrorOr;

namespace Application.Features.Documents.Command.AddDocuments; 

public sealed record AddDocumentsCommand(IEnumerable<DocumentDtoCreate> Documents) : IRequest<IEnumerable<DocumentDto>>;
