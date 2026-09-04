using Application.Interfaces.Customers;
using Application.Responses;
using Domains.Models.Lists.Customers;
using ErrorOr;
using Mapster;

namespace Application.Features.Activities.Command.CreateActivity
{
    public sealed class AddCustomerActivityUseCase
    {
        private readonly ICustomerActivityService _actService;

        public AddCustomerActivityUseCase(ICustomerActivityService noteService)
        {
            _actService = noteService;
        }

        public async Task<ErrorOr<int>> ExecuteAsync(ActivityDtoCreate req, CancellationToken cancellation = default)
        {
            var model = req.Adapt<CustomerActivity>();

            return await _actService.AddOrUpdateActivity(model, cancellation);
        }
    }

}
