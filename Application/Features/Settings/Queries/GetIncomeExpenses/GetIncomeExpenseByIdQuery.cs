using Application.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Application.Features.Settings.Queries.GetIncomeExpenses;

public record GetIncomeExpenseByIdQuery(int? Id) : IRequest<ErrorOr<IncomeExpense>>;