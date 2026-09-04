using Application.Features.Dashbaord.Dto;
using ErrorOr;

namespace Application.Features.Dashbaord.CreditOfficers.Queries.GetNumberLoanRequest;

public record GetNumberLoanRequestQuery(int? BranchId, string? UserId) : IRequest<ErrorOr<NumerLoanRequest>>;
