namespace Applications.Requests.Loans.LoanRequests;


public sealed class RecheckedRequestDtoCreate : RequestDtoBaseCreate
{
    public RecheckedRequestDtoCreate(int? branchId, string? customerId, string? loanRequestId, DateTime? checkedDate, string? memo)
        : base(branchId, customerId, loanRequestId, memo)
    {
        CheckedDate = checkedDate;
    }
    public DateTime? CheckedDate { get; set; } = DateTime.UtcNow;
}