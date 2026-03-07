using Applications.Calculations;
using Applications.Features.Loans.Requests;

namespace Applications.Responses.Loans.Disbursements;

public static class LoanCreateExtensions
{
    public static LoanInfo ToLoanInfo(this LoanDtoCreate arg)
    {
        return new LoanInfo(arg.PayEvery, arg.CollectionFeeRate, arg.CollectionFeeModeId, arg.InstallmentTypeId, arg.FirstPaymentDate, arg.StartDate);
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