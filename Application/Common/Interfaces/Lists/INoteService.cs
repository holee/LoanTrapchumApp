using Application.Responses;
using Application.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Application.Interfaces.Lists
{
    public interface INoteService
    {
        Task<Result<string>> AddNotesAsync(CustomerNote customerNote);
        Task<Result<VCustomerNotes>> GetCustomerNoteAsync(int noteId);
        Task<Result<IEnumerable<VCustomerNotes>>> GetCustomerNotesAsync(long customerId, int noteId = 0);
        Task<Result<string>> UpdateNotesAsync(CustomerNote customerNote);
    }
}
