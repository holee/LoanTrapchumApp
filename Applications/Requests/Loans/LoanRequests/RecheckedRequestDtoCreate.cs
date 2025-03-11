namespace Applications.Requests.Loans.LoanRequests;


public sealed class RecheckedRequestDtoCreate : RequestDtoBase
{
    public RecheckedRequestDtoCreate(int? branchId, long? customerId, long? loanRequestId, DateTime? checkedDate, string? memo)
        : base(branchId, customerId, loanRequestId, memo)
    {
        CheckedDate = checkedDate;
    }
    public DateTime? CheckedDate { get; set; } = DateTime.UtcNow;
}