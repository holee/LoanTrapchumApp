using Application.Calculations;

namespace Application.Features.LoanPayments.Requests;

public sealed class LoanPaymentDtoCreate
{
    public string? CustomerName { get; set; }
    public string? TransactionRefId { get; set; }
    public string? LoanRefId { get; set; }
    public string? CreditAgentRefId { get; set; }
    public string? UserRefId { get; set; }
    public int? BranchRefId { get; set; }
    public int? CurrencyRefId { get; set; }
    public string? OfficialReceiptNo { get; set; }
    public DateTime? PayDate { get; set; }
    public int? InstallmentNo { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFeeAmount { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? SubTotal { get; set; }
    public decimal? AmountWithoutPenalty => LoanPaymentLines.GetTotal(includePenalty: false);
    public decimal? AmountWithPenalty => LoanPaymentLines.GetTotal(includePenalty: true);
    public decimal? Amount { get; set; }
    public decimal? PaidPrinciple => LoanPaymentLines.TotalPaidPrinciple();
    public decimal? PaidInterest => LoanPaymentLines.TotalPaidInterest();
    public decimal? PaidFeeAmount => LoanPaymentLines.TotalPaidCollectionFee();
    public decimal? PaidPenaltyAmount => LoanPaymentLines.TotalPaidPenalty();
    public decimal? TotalAmount => LoanPaymentLines.GetTotal(includePenalty: true);
    public decimal? RecieveAmount { get; set; } = 0;
    public decimal? ChangeAmount { get; set; } = 0;
    public ICollection<LoanPaymentLineDtoCreate> LoanPaymentLines { get; set; } = [];

}


internal static class LoanPaymentDtoCreateExtension
{
    public static decimal? GetTotal(this IEnumerable<LoanPaymentLineDtoCreate> lines, bool includePenalty = false)
    {
        return !includePenalty ? lines.GetTotalNoPenalty() : lines.GetTotalWithPenalty();
    }
    private static decimal? GetTotalWithPenalty(this IEnumerable<LoanPaymentLineDtoCreate> lines)
    {
        var totalPrinciples = lines.Sum(x => x.PaidPrinciple) ?? 0;
        var totalInterest = lines.Sum(x => x.PaidInterest) ?? 0;
        var totalFee = lines.Sum(x => x.PaidCollectionFee) ?? 0;
        var totalPenalty = lines.Sum(x => x.PaidPenaltyAmount) ?? 0;
        return totalPrinciples + totalInterest + totalFee + totalPenalty;
    }

    private static decimal? GetTotalNoPenalty(this IEnumerable<LoanPaymentLineDtoCreate> lines)
    {
        var totalPrinciples = lines.Sum(x => x.PaidPrinciple) ?? 0;
        var totalInterest = lines.Sum(x => x.PaidInterest) ?? 0;
        var totalFee = lines.Sum(x => x.PaidCollectionFee) ?? 0;
        return totalPrinciples + totalInterest + totalFee;
    }
}
