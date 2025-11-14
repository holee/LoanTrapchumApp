using Applications.Features.Incomes.Dto;
using Applications.Responses.Paginations;
using Applications.Utils;
using Domains.Enumerations;

namespace Applications.Features.Incomes.Queries.GetIncomeExpense;

public record GetIncomeExpenseQuery(int? Id, IncomeExpenseTypes Type, QueryParam Query) : IRequest<Pager<IncomeExpenseDto>>;
