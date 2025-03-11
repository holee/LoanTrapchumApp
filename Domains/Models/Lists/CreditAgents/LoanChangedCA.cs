namespace Domains.Models.Lists.CreditAgents
{
    public class LoanChangedCa
    {
        public int Id { get; set; }
        public long? LoanRefId { get; set; }
        public long? OldCreditAgentRefId { get; set; }
        public long? NewCreditAgentRefId { get; set; }
        public long? StaffId { get; set; }
        public long? StaffAuthId  { get; set; }
        public DateTime? MonetaDate { get; set; }
        public DateTime? WindowDate { get; set; }
    }
}
