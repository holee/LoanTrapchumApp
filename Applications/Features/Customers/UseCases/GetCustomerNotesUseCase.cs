using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;

namespace Applications.Features.Customers.UseCases
{
    public sealed class GetCustomerNotesUseCase
    {
        private readonly ICustomerNoteService noteService;

        public GetCustomerNotesUseCase(ICustomerNoteService noteService)
        {
            this.noteService = noteService;
        }

        public async Task<Result<IEnumerable<VCustomerNotes>>> ExecuteAsync(string? customerId, CancellationToken cancellation = default)
        {
            var items = await noteService.GetCustomerNotes<VCustomerNotes>(customerId, cancellation);

            return items;
        }
    }

}
