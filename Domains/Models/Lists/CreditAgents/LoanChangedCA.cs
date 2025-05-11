namespace Domains.Models.Lists.CreditAgents
{
    public class LoanChangedCa
    {
        public int Id { get; set; }
        public string? LoanRefId { get; set; }
        public string? OldCreditAgentRefId { get; set; }
        public string? NewCreditAgentRefId { get; set; }
        public string? StaffId { get; set; }
        public string? StaffAuthId  { get; set; }
        public DateTime? MonetaDate { get; set; }
        public DateTime? WindowDate { get; set; }
    }
}
