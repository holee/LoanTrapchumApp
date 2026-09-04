namespace Application.Features.CreditAgents.Responses
{
    public sealed class LoanChangedCaDtoCreate
    {
        public LoanChangedCaDtoCreate(){}

        public LoanChangedCaDtoCreate(string? loanRefId, string? oldCreditAgentRefId, string? newCreditAgentRefId, string? staffId, string? staffAuthId)
        {
            LoanRefId = loanRefId;
            OldCreditAgentRefId = oldCreditAgentRefId;
            NewCreditAgentRefId = newCreditAgentRefId;
            StaffId = staffId;
            StaffAuthId = staffAuthId;
        }

        public string? LoanRefId { get; set; }
        public string? OldCreditAgentRefId { get; set; }
        public string? NewCreditAgentRefId { get; set; }
        public string? StaffId { get; set; }
        public string? StaffAuthId { get; set; }
    }
}
