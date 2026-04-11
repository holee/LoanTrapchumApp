using Applications.Commons.Mappers.Customers;
using Applications.Interfaces.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<VCustomerList>>
{
    private readonly ICustomerService _customerRepository;

    public CreateCustomerCommandHandler(ICustomerService customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<ErrorOr<VCustomerList>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {

        var customer = request.ToCustomerCreate(); 

        return await _customerRepository.CreateAsync(customer, cancellationToken);

    }
}