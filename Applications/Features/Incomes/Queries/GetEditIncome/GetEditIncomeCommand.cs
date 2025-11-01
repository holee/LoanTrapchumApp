using Applications.Features.Incomes.Dto;
using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Incomes.Queries.GetEditIncome;

public record GetEditIncomeCommand(long? Id, string? CustomerId) : IRequest<ErrorOr<EditIncomeDto>>;
