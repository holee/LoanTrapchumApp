using Applications.Features.Expenses.Dto;
using Applications.Features.Incomes.Dto;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Domains.Enumerations;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Interfaces.Customers
{
    public interface ICustomerIncomeExpendService
    {
        Task<ErrorOr<string>> DeleteExpensesAsync(long? id, string? customerRefId,
            CancellationToken cancellation = default);

        Task<ErrorOr<string>> AddOrUpdateExpensesAsync(string? customerId, List<CustomerExpense> req);
        Task<ErrorOr<long>> AddOrUpdateExpenseAsync(CustomerExpense req, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<VCustomerExpenses>>> GetExpenses(string? customerId);

        Task<ErrorOr<VCustomerExpenses>> GetExpense(long? id, string? customerId,
            CancellationToken cancellation = default);

        Task<ErrorOr<ExpenseDtoUpdate>> GetEditExpensesAsync(long? id, string? customerId,
            CancellationToken cancellation = default);


        Task<ErrorOr<string>> AddOrUpdateIncomesAsync(string? customerId, List<CustomerIncome> req);

        Task<ErrorOr<VCustomerIncomes>> GetIncomeByIdAsync(long? id, string? customerId);
        Task<ErrorOr<IEnumerable<VCustomerIncomes>>> GetIncomesAsync(string? customerId);
        Task<ErrorOr<long>> AddOrUpdateIncomeAsync(CustomerIncome req, CancellationToken cancellation = default);

        Task<ErrorOr<bool>> DeleteIncomeAsync(long? id, string? customerId, CancellationToken cancellationToken = default);

        Task<ErrorOr<EditIncomeDto>> GetEditIncomeAsync(long? id, string? customerId, CancellationToken cancellation = default);

        Task<ErrorOr<int>> AddOrUpdateIncomeExpenseAsync(IncomeExpense req, CancellationToken cancellation = default);

        Task<Pager<IncomeExpenseDto>> GetIncomeExpenseAsPagingAsync(int? id, IncomeExpenseTypes type, QueryParam query,
            CancellationToken cancellation = default);

        Task<ErrorOr<IncomeExpenseDto>> GetEditIncomeExpenseAsync(int? id, CancellationToken cancellation = default);
        Task<ErrorOr<IncomeExpense>> GetIncomeExpenseAsync(int? requestId, CancellationToken cancellationToken = default);
    }
}

//Task<Result<string>> AddIncomesAsync(string? customerRefId, List<CustomerIncome> req);
//Task<Result<string>> AddExpensesAsync(string? customerRefId, List<CustomerExpense> req);