using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;
using Application.Commons.Mappers.Incomes;
using Application.Features.Incomes.Command.CreateIncomeExpenseSource;

namespace Application.Features.Incomes.Command.EditIncomeExpenseSource;

public sealed class EditIncomeExpenseCommandHandler : IRequestHandler<EditIncomeExpenseCommand, ErrorOr<int>>
{


    private readonly ICustomerIncomeExpendService _expendService;

    public EditIncomeExpenseCommandHandler(ICustomerIncomeExpendService expendService)
    {
        _expendService = expendService;
    }

    public Task<ErrorOr<int>> Handle(EditIncomeExpenseCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.Request.ToIncomeExpenseEdit();

        return _expendService.AddOrUpdateIncomeExpenseAsync(model,cancellationToken);
    }
}