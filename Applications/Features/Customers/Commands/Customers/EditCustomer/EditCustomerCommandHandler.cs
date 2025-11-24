using Applications.Commons.Mappers.Customers;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.Customers.CreateCustomer;

public sealed class EditCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<string>>
{
    private readonly ICustomerService _customerRepository;

    public EditCustomerCommandHandler(ICustomerService customerRepository) 
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try 
        {
            var customer = request.ToCustomerCreate();

            return await _customerRepository.UpdateAsync(customer, cancellationToken);

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }
    }
}