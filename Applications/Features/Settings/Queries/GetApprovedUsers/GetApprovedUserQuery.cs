using ErrorOr;

namespace Applications.Features.Settings.Queries.GetApprovedUsers;

public record GetApprovedUserQuery(int? BranchId, int? CurrencyId, decimal? Amount, QueryParam? Param)
    : IRequest<ErrorOr<IEnumerable<VUserLoanSizes>>>;
