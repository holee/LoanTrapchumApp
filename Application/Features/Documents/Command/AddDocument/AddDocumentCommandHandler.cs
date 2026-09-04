using Application.Commons.Mappers;
using Application.Features.Documents.Dto;
using Application.Interfaces;
using Domains.Models;
using ErrorOr;

namespace Application.Features.Documents.Command.AddDocument;

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
