using Applications.Interfaces.Customers;
using ErrorOr;

namespace Applications.Features.Incomes.Command.DeleteIncomeExpense
{
    public class DeleteIncomeExpenseCommandHandler : IRequestHandler<DeleteIncomeExpenseCommand, ErrorOr<string>>
    {
        private readonly ICustomerIncomeExpendService _service;
        public DeleteIncomeExpenseCommandHandler(ICustomerIncomeExpendService service)
        {
            _service = service;
        }
        public async Task<ErrorOr<string>> Handle(DeleteIncomeExpenseCommand request, CancellationToken cancellationToken)
        {
            return await _service.DeleteIncomeExpenseAsync(request.Id, cancellationToken);
        }
    }
}
