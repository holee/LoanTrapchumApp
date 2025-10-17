using Applications.Exceptions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using FluentValidation;

namespace Applications.Features.Customers.Commands.CustomerExpenseQuery;

public sealed class GetCustomerExpenseQueryHandler : IRequestHandler<GetCustomerExpenseQuery, IEnumerable<VCustomerExpenses>>
{
    private readonly ICustomerIncomeExpendService _customer;

    private readonly IValidator<GetCustomerExpenseQuery> _validator;
    public GetCustomerExpenseQueryHandler(ICustomerIncomeExpendService customer, IValidator<GetCustomerExpenseQuery> validator)
    {
        _customer = customer;
        _validator = validator;
    }

    public async Task<IEnumerable<VCustomerExpenses>> Handle(GetCustomerExpenseQuery request, CancellationToken cancellation = default)
    {
        var validator = await _validator.ValidateAsync(request, cancellation);

        if (!validator.IsValid)
        {
            throw new BadRequestExceptions(validator);
        }

        var rsp = await _customer.GetCustomerCustomerExpenses(request.CustomerId);

        return rsp.IsSuccess && rsp.Data is not null ? rsp.Data : [];

    }
}
