using Applications.Interfaces.Customers;
using Applications.Responses;
using Domains.Models.Lists.Customers;
using Mapster;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity
{
    public sealed class EditCustomerActivityUseCase
    {
        private readonly ICustomerActivityService _actService;

        public EditCustomerActivityUseCase(ICustomerActivityService noteService)
        {
            _actService = noteService;
        }

        public async Task<Result<string>> ExecuteAsync(ActivityDtoCreate req, CancellationToken cancellation = default)
        {
            var model = req.Adapt<CustomerActivity>();

            return await _actService.AddOrUpdateActivity(model, cancellation);
        }
    }

}
