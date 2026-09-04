using Application.Commons.Mappers;
using Application.Features.Documents.Dto;
using Application.Interfaces;
using Domains.Models;
using ErrorOr;

namespace Application.Features.Documents.Command.AddDocuments
{
    public sealed class AddDocumentsCommandHandler : IRequestHandler<AddDocumentsCommand, IEnumerable<DocumentDto>>
    {
        private readonly IDocumentServices _documentServices;

        public AddDocumentsCommandHandler(IDocumentServices documentServices)
        {
            _documentServices = documentServices;
        }

        public async Task<IEnumerable<DocumentDto>> Handle(AddDocumentsCommand request, CancellationToken cancellationToken = default)
        {
            var documents = request.Documents.Select(d => d.ToDocument()).ToList();

            var result = await _documentServices.AddDocumentsAsync(documents, cancellationToken);

            if (result is null) return [];

            var dataMappers = result.Select(d => d.ToDocumentDto()).ToList();

            return dataMappers;
        }
    }
}
