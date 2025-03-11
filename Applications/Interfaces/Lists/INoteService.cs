using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Lists
{
    public interface INoteService
    {
        Task<Result<string>> AddNotesAsync(CustomerNote customerNote);
        Task<Result<VCustomerNotes>> GetCustomerNoteAsync(int noteId);
        Task<Result<IEnumerable<VCustomerNotes>>> GetCustomerNotesAsync(long customerId, int noteId = 0);
        Task<Result<string>> UpdateNotesAsync(CustomerNote customerNote);
    }
}
