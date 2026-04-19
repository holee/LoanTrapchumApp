using Applications.Commons.Mappers;
using Applications.Features.Documents.Dto;
using Applications.Interfaces;
using Domains.Models;
using ErrorOr;

namespace Applications.Features.Documents.Command.AddDocument;

public sealed class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand, ErrorOr<DocumentDto>>
{
    private readonly IDocumentServices _documentServices;

    public AddDocumentCommandHandler(IDocumentServices documentServices)
    {
        _documentServices = documentServices;
    }

    public async Task<ErrorOr<DocumentDto>> Handle(AddDocumentCommand request, CancellationToken cancellationToken = default)
    {
        LtDocument document = request.Document.ToDocument();

        var result = await _documentServices.AddDocumentAsync(document, cancellationToken);

        if (result is null)
            return Error.Failure(description: "Failed to add document.");

        var dataMapper = result.ToDocumentDto();

        return dataMapper;
    }
}
