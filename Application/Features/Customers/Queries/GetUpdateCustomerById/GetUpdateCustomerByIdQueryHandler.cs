using Application.Commons.Mappers.Customers;
using Application.Features.Customers.Dto;
using Application.Interfaces.Customers;
using ErrorOr;

namespace Application.Features.Customers.Queries.GetUpdateCustomerById
{
    public sealed class GetUpdateCustomerByIdQueryHandler : IRequestHandler<GetUpdateCustomerByIdQuery, ErrorOr<EntityCustomerCreate>>
    {
        private readonly ICustomerService _custService;

        public GetUpdateCustomerByIdQueryHandler(ICustomerService custService)
        {
            _custService = custService;
        }


        public async Task<ErrorOr<EntityCustomerCreate>> Handle(GetUpdateCustomerByIdQuery request, CancellationToken cancellationToken = default)
        {

            var rsp = await _custService.GetCustomerByIdAsync(request.CustomerId);

            if (rsp == null) return Error.NotFound(description: "Customer information was changed successfuly");

            return rsp.ToCustomer();
        }
    }
}
