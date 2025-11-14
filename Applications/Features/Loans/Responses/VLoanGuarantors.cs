namespace Applications.Features.Loans.Responses;

public sealed class VLoanGuarantors 
{

    public long Id { get; set; }              
    public string? LoanId { get; set; }              
    public string? LoanRequestId { get; set; }       
    public string? GuarantorId { get; set; }        
    public string? FullName { get; set; }         
    public string? GuarantorName { get; set; } 

    //public long Id { get; set; }
    //public string? GuarantorId { get; set; }
    //public string? GuarantorName { get; set; }
    //public string? GuarantorKhmerName { get; set; }
    //public string? LoanId { get; set; }
    //public string? CustomerName { get; set; }
    //public string? CustomerKhmerName { get; set; }
    //public int? Installments { get; set; }
    //public string? InstallmentName { get; set; }
    //public string? InstallmentType { get; set; }
    //public decimal? Principle { get; set; }
    //public int? CurrencyId { get; set; } 
    //public decimal? InterestRate { get; set; }
    //public decimal? CollectionFeeRate { get; set; }
    //public decimal? AdminFeeRate { get; set; }
    //public string? CreditAgentName { get; set; }
    //public string? CreditAgentKhmerName { get; set; }
    //public string? Remark { get; set; }
}
