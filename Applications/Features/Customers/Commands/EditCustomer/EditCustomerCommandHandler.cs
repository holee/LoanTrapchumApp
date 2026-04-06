using Applications.Commons.Mappers.Customers;
using Applications.Interfaces.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Commands.EditCustomer;

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