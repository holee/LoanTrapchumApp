using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;
using Applications.Commons.Mappers.Incomes;
namespace Applications.Features.Incomes.Command.CreateIncomeExpenseSource;

public sealed class CreateIncomeExpenseCommandHandler : IRequestHandler<CreateIncomeExpenseCommand, ErrorOr<int>>
{

    private readonly ICustomerIncomeExpendService _expendService;

    public CreateIncomeExpenseCommandHandler(ICustomerIncomeExpendService expendService)
    {
        _expendService = expendService;
    }

    public Task<ErrorOr<int>> Handle(CreateIncomeExpenseCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.Request.ToIncomeExpenseCreate();

        return _expendService.AddOrUpdateIncomeExpenseAsync(model,cancellationToken);
    }
}