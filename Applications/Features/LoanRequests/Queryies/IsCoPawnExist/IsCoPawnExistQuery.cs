using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.IsCoPawnExist;

public record IsCoPawnExistQuery(int? BranchId, string? CustomerId) : IRequest<ErrorOr<bool>>;
