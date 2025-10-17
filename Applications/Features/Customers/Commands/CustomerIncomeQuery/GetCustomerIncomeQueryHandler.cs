using Applications.Exceptions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using FluentValidation;

namespace Applications.Features.Customers.Commands.CustomerIncomeQuery;

public sealed class GetCustomerIncomeQueryHandler : IRequestHandler<GetCustomerIncomeQuery, IEnumerable<VCustomerIncomes>>
{
    private readonly ICustomerIncomeExpendService _customer;

    private readonly IValidator<GetCustomerIncomeQuery> _validator;

    public GetCustomerIncomeQueryHandler(ICustomerIncomeExpendService customer, IValidator<GetCustomerIncomeQuery> validator)
    {
        _customer = customer;
        _validator = validator;
    }

    public async Task<IEnumerable<VCustomerIncomes>> Handle(GetCustomerIncomeQuery request, CancellationToken cancellationToken = default)
    {

        var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validatorResult.IsValid)
        {
            throw new BadRequestExceptions(validatorResult);
        }

        var rsp = await _customer.GetCustomerCustomerIncomes(request.CustomerId);

        return rsp.IsSuccess && rsp.Data is not null ? rsp.Data : [];
    }
}
