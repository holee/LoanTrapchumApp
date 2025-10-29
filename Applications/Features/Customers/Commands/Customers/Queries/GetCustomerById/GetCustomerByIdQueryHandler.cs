using Applications.Commons.Messages.Customers;
using Applications.Interfaces.Customers;
using Applications.Utils;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Customers.Queries.GetCustomerById
{
    public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ErrorOr<EntityNames>>
    {
        private readonly ICustomerService _custService;

        public GetCustomerByIdQueryHandler(ICustomerService custService)
        {
            _custService = custService;
        }


        public async Task<ErrorOr<EntityNames>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                return Error.NotFound(CustomerMessage.Required);
            }

            var rsp = await _custService.GetCustomerByIdAsync(request.CustomerId);


            if (rsp.IsSuccess && rsp.Data is not null)
            {
                return rsp.Data;
            }
            else
            {
                return Error.NotFound(CustomerMessage.NotFound);
            }
        }
    }
}
