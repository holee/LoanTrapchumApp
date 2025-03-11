using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Services.Lists
{
    public sealed class NoteService(INoteService nService) : INoteService
    {
        public async Task<Result<string>> AddNotesAsync(CustomerNote customerNote)
        {
            return await nService.AddNotesAsync(customerNote);    
        }

        public async Task<Result<VCustomerNotes>> GetCustomerNoteAsync(int noteId)
        {
            return await nService.GetCustomerNoteAsync(noteId);
        }

        public Task<Result<IEnumerable<VCustomerNotes>>> GetCustomerNotesAsync(long customerId, int noteId = 0)
        {
            return nService.GetCustomerNotesAsync(customerId, noteId);
        }

        public async Task<Result<string>> UpdateNotesAsync(CustomerNote customerNote)
        {
            return await nService.UpdateNotesAsync(customerNote);
        }
    }
}
