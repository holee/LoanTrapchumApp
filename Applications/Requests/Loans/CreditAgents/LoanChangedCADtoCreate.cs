namespace Applications.Requests.Loans.CreditAgents;

public sealed class LoanChangedCaDtoCreate(Guid? loanRefId, Guid? oldCreditAgentRefId,
    Guid? newCreditAgentRefId, Guid? staffId, Guid? staffAuthId)
{
    public Guid? LoanRefId { get; } = loanRefId;
    public Guid? OldCreditAgentRefId { get; } = oldCreditAgentRefId;
    public Guid? NewCreditAgentRefId { get; } = newCreditAgentRefId;
    public Guid? StaffId { get; } = staffId;
    public Guid? StaffAuthId { get; } = staffAuthId;
}
