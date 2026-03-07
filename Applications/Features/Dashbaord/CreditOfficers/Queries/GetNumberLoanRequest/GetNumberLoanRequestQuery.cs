using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using ErrorOr;

namespace Applications.Features.Dashbaord.CreditOfficers.Queries.GetNumberLoanRequest;

public record GetNumberLoanRequestQuery(int? BranchId, string? UserId) : IRequest<ErrorOr<NumerLoanRequest>>;
