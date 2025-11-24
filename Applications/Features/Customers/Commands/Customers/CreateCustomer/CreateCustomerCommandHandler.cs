using Applications.Commons.Mappers.Customers;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.Customers.CreateCustomer;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<string>>
{
    private readonly ICustomerService _customerRepository;

    public CreateCustomerCommandHandler(ICustomerService customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = request.Request.ToCustomerCreate();

            cancellationToken.ThrowIfCancellationRequested();

            return await _customerRepository.CreateAsync(customer, cancellationToken);

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }
    }
}