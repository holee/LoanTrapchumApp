using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Activities.Command.EditActivity
{
    public sealed class EditActivityCommandHandler : IRequestHandler<EditActivityCommand,ErrorOr<int>>
    {
        private readonly ICustomerActivityService _actService;
        
        public EditActivityCommandHandler(ICustomerActivityService actService)
        {
            _actService = actService;
        }

        public async Task<ErrorOr<int>> Handle(EditActivityCommand request, CancellationToken cancellationToken = default)
        {
            var model = request.ToActivity();
            return await _actService.AddOrUpdateActivity(model, cancellationToken);

        }
    }
}
