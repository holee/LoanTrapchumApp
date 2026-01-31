using Applications.Commons.Mappers.Customers;
using Applications.Features.Customers.Commands.Customers.EditCustomer;
using Applications.Interfaces.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Customers.CreateCustomer;

public sealed class EditCustomerCommandHandler : IRequestHandler<EditCustomerCommand, ErrorOr<VCustomerList>>
{
    private readonly ICustomerService _customerRepository;

    public EditCustomerCommandHandler(ICustomerService customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<ErrorOr<VCustomerList>> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
    {

        var customer = request.ToCustomerEdit();

        return await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);
    }
}