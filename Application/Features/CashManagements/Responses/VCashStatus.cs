namespace Application.Features.CashManagements.Responses
{
    public class VCashStatus
    {
        public long? CashManagementId { get; set; }
        public int? BranchRefId { get; set; }
        public DateTime? BusinessDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int CashStatusRefId { get; set; }
        public string? Remark { get; set; }
    }
}
