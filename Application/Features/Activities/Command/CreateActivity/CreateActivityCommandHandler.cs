using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Activities.Command.CreateActivity
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

            return await _actService.AddOrUpdateActivity(model, cancellationToken);            
        }
    }
}
