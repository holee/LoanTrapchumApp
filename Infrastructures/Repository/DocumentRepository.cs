using Applications.Interfaces;
using Domains.Models;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructures.Repository;

public sealed class DocumentRepository(DataContext contextFactory, ILogger<DocumentRepository> logger) : IDocumentServices
{
    public async Task<LtDocument> AddDocumentAsync(LtDocument document, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Adding document with original name: {OriginalName}", document.OriginalName);

            var context = contextFactory;

            context.LtDocuments.Add(document);

            await context.SaveChangesAsync(cancellation);

            return document;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding the document with original name: {OriginalName}", document.OriginalName);

            throw;
        }


    }


    public async Task<IEnumerable<LtDocument>> AddDocumentsAsync(IEnumerable<LtDocument> documents, CancellationToken cancellation = default)
    {
        logger.LogInformation("Adding {Count} documents", documents.Count());

        try
        {

            var context = contextFactory;

            await context.LtDocuments.AddRangeAsync(documents, cancellation);

            await context.SaveChangesAsync(cancellation);

            return documents;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding {Count} documents", documents.Count());

            throw;
        }

    }

    public async Task<LtDocument?> GetDocumentByIdAsync(Guid documentId, CancellationToken cancellation)
    {
        logger.LogInformation("Retrieving document with ID: {DocumentId}", documentId);

        var context = contextFactory;

        var document = await context.LtDocuments
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(d => d.Id == documentId, cancellation);
        return document;

    }

    public async Task<IEnumerable<LtDocument>> GetDocumentsAsync(Expression<Func<LtDocument, bool>>? filter = null, CancellationToken cancellation = default)
    {
        logger.LogInformation("Retrieving documents. HasFilter: {HasFilter}", filter != null);

        var context = contextFactory;

        IQueryable<LtDocument> query = context.LtDocuments.AsNoTracking();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellation);
    }

    public async Task<bool> DeleteDocumentAsync(Guid documentId, CancellationToken cancellation = default)
    {
        logger.LogInformation("Deleting document with ID: {DocumentId}", documentId);

        try
        {
            var context = contextFactory;

            var entity = await context.LtDocuments.FirstOrDefaultAsync(d => d.Id == documentId, cancellation);

            if (entity is null)
            {
                logger.LogWarning("Document not found: {DocumentId}", documentId);
                return false;
            }

            context.LtDocuments.Remove(entity);

            await context.SaveChangesAsync(cancellation);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting document with ID: {DocumentId}", documentId);
            throw;
        }
    }


}