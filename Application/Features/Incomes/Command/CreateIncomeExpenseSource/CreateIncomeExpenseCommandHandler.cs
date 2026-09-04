using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;
using Application.Commons.Mappers.Incomes;
namespace Application.Features.Incomes.Command.CreateIncomeExpenseSource;

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