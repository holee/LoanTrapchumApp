using Domains.Models;
using System.Linq.Expressions;

namespace Applications.Interfaces
{
    public interface IDocumentServices
    {
        Task<LtDocument> AddDocumentAsync(LtDocument document, CancellationToken cancellation = default);
        Task<IEnumerable<LtDocument>> AddDocumentsAsync(IEnumerable<LtDocument> documents, CancellationToken cancellation = default);
        Task<bool> DeleteDocumentAsync(Guid documentId, CancellationToken cancellation = default);
        Task<LtDocument?> GetDocumentByIdAsync(Guid documentId, CancellationToken cancellation);
        Task<IEnumerable<LtDocument>> GetDocumentsAsync(Expression<Func<LtDocument, bool>>? filter = null, CancellationToken cancellation = default);
    }
}