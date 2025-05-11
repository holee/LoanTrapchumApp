namespace Applications.Responses.ViewModels.Loans;

public class VLoanRepaymentLine 
{
    public long? Id { get; set; }
    public long? LoanScheduleRefId { get; set; }  
    public string? LoanRefId { get; set; }
    public int InstallmentNo { get; set; }
    public decimal PaidPrinciple { get; set; }
    public decimal PaidInterest { get; set; }
    public decimal PaidCollectionFee { get; set; }
    public decimal PenaltyAmount { get; set; }
    public decimal SubTotal { get; set; }
}
