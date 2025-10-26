using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeByCustomerId;

public sealed record GetIncomeByIdQuery(long Id, string CustomerId) : IRequest<ErrorOr<VCustomerIncomes>>;
