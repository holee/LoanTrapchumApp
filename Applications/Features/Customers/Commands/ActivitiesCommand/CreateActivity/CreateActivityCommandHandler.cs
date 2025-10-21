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
    public sealed class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand,ErrorOr<int>>
    {
        private readonly ICustomerActivityService _actService;

        public async Task<ErrorOr<int>> Handle(CreateActivityCommand request, CancellationToken cancellationToken = default)
        {
            var model = request.Adapt<CustomerActivity>();

            return await _actService.AddOrUpdateActivity(model, cancellationToken);
            
        }
    }
}
