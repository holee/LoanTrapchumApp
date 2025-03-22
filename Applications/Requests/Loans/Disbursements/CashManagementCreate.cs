namespace Applications.Requests.Loans.Disbursements;

public sealed class CashManagementCreate
{
    public int StaffRefId { get; set; }
    public int CashStatusRefId { get; set; }
    public int CurrencyRefId { get; set; }
    public int BranchRefId { get; set; }
    public int BankRefId { get; set; }
    public int Balance { get; set; }
    public int BusinessDate { get; set; }
    public int TransactionDate { get; set; }
    public int Reject { get; set; }
    public int Receive { get; set; }
    public int Remark { get; set; }
    public int Amount { get; set; }

}



//public List<GuarantorDtoCreate>? LoanGuarantors { get; set; } = [];
//public List<CollateralLineDtoCreate>? LoanCollateralLines { get; set; } = [];
//public decimal? ArrearRate { get; set; }
//public decimal? PaidPrinciple { get; set; }
// public decimal? PaidInterest { get; set; }
//public int? PaidInstallment { get; set; }
// public decimal? InsInstallment { get; set; }
//public int? PaymentLocationId { get; set; }