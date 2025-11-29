namespace Applications.Features.Loans.Responses
{
    public sealed record DisburseResult(bool Success, string? LoanId, int? BranchId, string? Message = null);
    public sealed record RequestResult(bool Success = false, string? Message = null);

}
