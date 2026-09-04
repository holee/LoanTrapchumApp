using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Queries.GetIncomeById;

public sealed record GetIncomeByIdQuery(long Id, string CustomerId) : IRequest<ErrorOr<VCustomerIncomes>>;
