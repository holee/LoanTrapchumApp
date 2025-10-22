using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity
{
    public sealed class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand,ErrorOr<int>>
    {
        private readonly ICustomerActivityService _actService;

        public CreateActivityCommandHandler(ICustomerActivityService actService)
        {
            _actService = actService;
        }

        public async Task<ErrorOr<int>> Handle(CreateActivityCommand request, CancellationToken cancellationToken = default)
        {
            var model = request.ToCreateActivity();

             await _actService.AddOrUpdateActivity(model, cancellationToken);

            return 1;
            
        }
    }
}
