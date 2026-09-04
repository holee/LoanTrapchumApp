using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.IsCoPawnExist;

public record IsCoPawnExistQuery(int? BranchId, string? CustomerId) : IRequest<ErrorOr<bool>>;
