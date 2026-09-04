using Application.Features.Incomes.Dto;
using Application.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Application.Features.Incomes.Queries.GetEditIncome;

public record GetEditIncomeCommand(long? Id, string? CustomerId) : IRequest<ErrorOr<EditIncomeDto>>;
