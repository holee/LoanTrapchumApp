using Applications.Features.Customers.Requests;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Domains.Models.Lists.Customers;
using Mapster;

namespace Applications.Features.Customers.UseCases
{
    public sealed class AddCustomerActivityUseCase
    {
        private readonly ICustomerActivityService _actService;

        public AddCustomerActivityUseCase(ICustomerActivityService noteService)
        {
            _actService = noteService;
        }

        public async Task<Result<string>> ExecuteAsync(CustomerActivityDtoCreate req, CancellationToken cancellation = default)
        {
            var model = req.Adapt<CustomerActivity>();

            return await _actService.AddCustomerActivity(model, cancellation);
        }
    }

}
