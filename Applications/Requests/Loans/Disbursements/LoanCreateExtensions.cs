using Domains.Models.Loans;

namespace Applications.Requests.Loans.Disbursements;

public static class LoanCreateExtensions
{
    public static LoanInfo ToLoanInfo(this LoanDtoCreate arg)
    {
        return new LoanInfo(arg.PayEvery!.Value, arg.CollectionFeeRate!.Value, arg.OtherChargeId!.Value, arg.InstallmentTypeId!.Value, arg.FirstPaymentDate!.Value, arg.StartDate!.Value);
    }
}



//public List<GuarantorDtoCreate>? LoanGuarantors { get; set; } = [];
//public List<CollateralLineDtoCreate>? LoanCollateralLines { get; set; } = [];
//public decimal? ArrearRate { get; set; }
//public decimal? PaidPrinciple { get; set; }
// public decimal? PaidInterest { get; set; }
//public int? PaidInstallment { get; set; }
// public decimal? InsInstallment { get; set; }
//public int? PaymentLocationId { get; set; }