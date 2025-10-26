using Applications.Commons.Messages.Incomes;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed class AddIncomesCommandHandler : IRequestHandler<AddIncomesCommand, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public AddIncomesCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<string>> Handle(AddIncomesCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDtos();

        var rsp = await _custInomeSerivce.AddIncomesAsync(request.CustomerId, model);

        if (rsp.IsSuccess)
        {
            return IncomeMessage.Created;
        }

        return Error.Unexpected(rsp.ErrMessage);

    }
}
