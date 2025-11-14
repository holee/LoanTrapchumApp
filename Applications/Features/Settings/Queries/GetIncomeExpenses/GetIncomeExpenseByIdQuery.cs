using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Settings.Queries.GetIncomeExpenses;

public record GetIncomeExpenseByIdQuery(int? Id) : IRequest<ErrorOr<IncomeExpense>>;