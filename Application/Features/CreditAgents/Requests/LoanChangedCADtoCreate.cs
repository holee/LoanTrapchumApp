namespace Application.Features.CreditAgents.Requests;

public sealed class LoanChangedCaDtoCreate(string? loanRefId, string? oldCreditAgentRefId,
    string? newCreditAgentRefId, string? staffId, string? staffAuthId)
{
    public string? LoanRefId { get; } = loanRefId;
    public string? OldCreditAgentRefId { get; } = oldCreditAgentRefId;
    public string? NewCreditAgentRefId { get; } = newCreditAgentRefId;
    public string? StaffId { get; } = staffId;
    public string? StaffAuthId { get; } = staffAuthId;
}
