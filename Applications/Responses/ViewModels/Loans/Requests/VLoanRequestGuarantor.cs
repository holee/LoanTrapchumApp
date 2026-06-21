using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Loans.Requests
{
    public sealed class VLoanRequestGuarantor
    {
        public long Id { get; set; }
        public string? GuarantorId { get; set; }
        public string? LoanRequestRefId { get; set; }
        public int? BranchId { get; set; }
        public string? GuarantorName { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; } = false;
        [NotMapped] public int RowIndex { get; set; } = 0;
    }

}

//public sealed class VLoanRequestGuarantor
//{
//    public long Id { get; set; }
//    public string? GuarantorId { get; set; }
//    public string? LoanRequestRefId { get; set; }
//    public decimal? RequestAmount { get; set; }
//    public decimal? AprovedAmount { get; set; }
//    public int? CurrencyId { get; set; }
//    public string? CurrencyName { get; set; }
//    public decimal? InterestRate { get; set; }
//    public decimal? CollectionFeeRate { get; set; }
//    public decimal? AdminFeeRate { get; set; }
//    public string? CreditAgentName { get; set; }
//    public string? AprovedBy { get; set; }
//    public string? CustomerName { get; set; }
//    public DateTime? RequestDate { get; set; }
//    public string? Remark { get; set; }
//    public string? Status { get; set; }
//}
