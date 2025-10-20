using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;
using Mapster;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity
{
    public sealed class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand,ErrorOr<VCustomerActivities>>
    {
        private readonly ICustomerActivityService _actService;

        public async Task<ErrorOr<VCustomerActivities>> Handle(CreateActivityCommand request, CancellationToken cancellationToken = default)
        {
            var model = request.Adapt<CustomerActivity>();

            await _actService.AddOrUpdateActivity(model, cancellationToken);

            return new ErrorOr<VCustomerActivities>();
        }
    }
}
