namespace Applications.Requests.Loans.Payments;

public sealed class LoanPaymentDtoCreate
{
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
    public decimal? AmountWithoutPenalty => LoanRepaymentLines.GetTotal();
    public decimal? AmountWithPenalty => LoanRepaymentLines.GetTotal(true);
    public decimal? Amount { get; set; }
    public ICollection<LoanPaymentLineDtoCreate> LoanRepaymentLines { get; set; } = [];
}


internal static class LoanPaymentDtoCreateExtension
{
    public static decimal? GetTotal(this IEnumerable<LoanPaymentLineDtoCreate> lines, bool includePenalty = false)
    {
        if (!includePenalty)
        {
            var totalPrinciples = lines.Sum(x => x.PaidPrinciple) ?? 0;
            var totalInterest = lines.Sum(x => x.PaidInterest) ?? 0;
            var totalFee = lines.Sum(x => x.PaidCollectionFee) ?? 0;
            return totalPrinciples + totalInterest + totalFee;
        }
        else
        {
            var totalPrinciples = lines.Sum(x => x.PaidPrinciple) ?? 0;
            var totalInterest = lines.Sum(x => x.PaidInterest) ?? 0;
            var totalFee = lines.Sum(x => x.PaidCollectionFee) ?? 0;
            var totalPenalty = lines.Sum(x => x.PenaltyAmount) ?? 0;
            return totalPrinciples + totalInterest + totalFee + totalPenalty;
        }

    }

    public static decimal? GetTotalWithPenalty(this IEnumerable<LoanPaymentLineDtoCreate> lines)
    {
        var totalPrinciples = lines.Sum(x => x.PaidPrinciple) ?? 0;
        var totalInterest = lines.Sum(x => x.PaidInterest) ?? 0;
        var totalFee = lines.Sum(x => x.PaidCollectionFee) ?? 0;
        var totalPenalty = lines.Sum(x => x.PenaltyAmount) ?? 0;
        return totalPrinciples + totalInterest + totalFee + totalPenalty;
    }
}

//public LoanPaymentTransactionDtoCreate? LoanTransaction { get; set; } = new();
//public TrxForeignExchangeDtoCreate? ForeignExchangeTrx { get; set; } = new();