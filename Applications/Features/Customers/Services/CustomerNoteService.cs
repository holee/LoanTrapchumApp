using Applications.Features.Customers.Mappers;
using Applications.Interfaces.Customers;
using Applications.Requests.Lists.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Customers;

namespace Applications.Features.Customers.Services
{
    public sealed class CustomerNoteService
    {
        private ICustomerService _customerService;

        public CustomerNoteService(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Result<IEnumerable<CustomerNoteResponse>>> GetCustomerNotes(string? customersId)
        {
            if (string.IsNullOrWhiteSpace(customersId)) 
            {
                return Result.Failure<IEnumerable<CustomerNoteResponse>>(ErrorResponse.Validation("NO Customer Notes Found"));
            }

            return await _customerService.GetCustomerNotes<CustomerNoteResponse>(customersId);
        }


        public async Task<Result<string>> AddCustomerNote(CustomerNoteDtoCreate req)  
        {

            var model = req.Create();

            return await _customerService.AddCustomerNote(model);
           
        }

    }
}
