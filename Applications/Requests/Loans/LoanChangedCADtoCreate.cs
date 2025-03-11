namespace Applications.Requests.Loans;

public sealed class LoanChangedCaDtoCreate(long? loanRefId, long? oldCreditAgentRefId, long? newCreditAgentRefId,
                              long? staffId, long? staffAuthId)
{
    public long? LoanRefId { get; } = loanRefId;
    public long? OldCreditAgentRefId { get; } = oldCreditAgentRefId;
    public long? NewCreditAgentRefId { get; } = newCreditAgentRefId;
    public long? StaffId { get; } = staffId;
    public long? StaffAuthId { get; } = staffAuthId;
}
