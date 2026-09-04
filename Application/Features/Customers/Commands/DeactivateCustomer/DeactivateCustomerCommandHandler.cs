using Application.Interfaces.Customers;
using ErrorOr;

namespace Application.Features.Customers.Commands.DeactivateCustomer;

public sealed class DeactivateCustomerCommandHandler : IRequestHandler<DeactivateCustomerCommand, ErrorOr<bool>>
{
    private readonly ICustomerService _customerService;
    public DeactivateCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<ErrorOr<bool>> Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
    {
        return await _customerService.MakeCustomerInActiveAsync(request.CustomerId, cancellationToken);
    }
}
