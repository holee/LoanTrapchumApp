using Applications.Commons.Messages.Incomes;
using Applications.Interfaces.Customers;
using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed class AddIncomeCommandHandler : IRequestHandler<AddIncomeCommand, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public AddIncomeCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<string>> Handle(AddIncomeCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDto();

        var rsp = await _custInomeSerivce.AddIncomesAsync(request.CustomerId, new List<CustomerIncome> { model });

        if (rsp.IsSuccess)
        {
            return IncomeMessage.Created;
        }

        return Error.Unexpected(rsp.ErrMessage);
    }
}
