using Application.Commons.Mappers.Incomes;
using Application.Commons.Messages.Incomes;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.CreateIncomes;

public sealed class CreateIncomesCommandHandler : IRequestHandler<CreateIncomesCommand, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public CreateIncomesCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<string>> Handle(CreateIncomesCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDtos();

        return await _custInomeSerivce.AddOrUpdateIncomesAsync(request.CustomerId, model);
    }
}
