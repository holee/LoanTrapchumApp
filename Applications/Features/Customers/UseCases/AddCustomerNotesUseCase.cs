using Applications.Features.Customers.Requests;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Domains.Models.Lists.Customers;
using Mapster;

namespace Applications.Features.Customers.UseCases
{

     
    public sealed class AddCustomerNotesUseCase
    {
        private readonly ICustomerNoteService noteService;

        public AddCustomerNotesUseCase(ICustomerNoteService noteService)
        {
            this.noteService = noteService;
        }

        public async Task<Result<string>> ExecuteAsync(CustomerNoteDtoCreate req, CancellationToken cancellation = default)
        {
            var model = req.Adapt<CustomerNote>();

            return await noteService.AddCustomerNote(model);
        }
    }

}
