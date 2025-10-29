using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Incomes.Queries.GetEditIncome;

public record GetEditIncomeCommand(long? Id, string? CustomerId) : IRequest<ErrorOr<CustomerIncome>>;
