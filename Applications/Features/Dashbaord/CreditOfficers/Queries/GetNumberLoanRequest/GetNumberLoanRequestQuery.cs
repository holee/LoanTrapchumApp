using Applications.Features.Dashbaord.Dto;
using ErrorOr;

namespace Applications.Features.Dashbaord.CreditOfficers.Queries.GetNumberLoanRequest;

public record GetNumberLoanRequestQuery(int? BranchId, string? UserId) : IRequest<ErrorOr<NumerLoanRequest>>;
