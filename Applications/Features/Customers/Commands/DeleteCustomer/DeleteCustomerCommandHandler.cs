using Applications.Interfaces.Customers;

namespace Applications.Features.Customers.Commands.DeleteCustomer;

public sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, DeleteResult>
{
    private readonly ICustomerService _customerService;
    public DeleteCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<DeleteResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        return await _customerService.DeleteAsync(request.CustomerId, cancellationToken);
    }
}
