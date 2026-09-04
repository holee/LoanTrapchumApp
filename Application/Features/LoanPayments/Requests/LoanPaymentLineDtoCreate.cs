using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.LoanPayments.Requests;

public sealed class LoanPaymentLineDtoCreate
{
    public long? RepaymentRefId { get; set; }
    public string? LoanRefId { get; set; }
    public int? BranchRefId { get; set; }
    public long? LoanScheduleRefId { get; set; }
    public string? OfficialReceiptNo { get; set; }
    public int? InstallmentNo { get; set; }
    public int? NumberOfDay { get; set; }
    public DateTime? PayDate { get; set; }= DateTime.Now;
    public int? DayArrea { get; set; } = 0;
    public decimal? PaidPrinciple { get; set; } = 0;
    public decimal? PaidInterest { get; set; } = 0;
    public decimal? PaidCollectionFee { get; set; } = 0;
    public decimal? PaidPenaltyAmount { get; set; } = 0;
    public decimal? SubTotal { get; set; } = 0;

    [NotMapped]
    public bool? IsEnable { get; set; }
    [NotMapped]
    public decimal? TotalWithPenalty => PaidPrinciple + PaidInterest + PaidCollectionFee + PaidPenaltyAmount;
    [NotMapped]
    public decimal? TotalWithNoPenalty => PaidPrinciple + PaidInterest + PaidCollectionFee;
    [NotMapped] public int RowIndex { get; set; } = 0;
}


public static class LoanPaymentLineDtoCreateExtension
{
    public static decimal? AsSubTotal(this LoanPaymentLineDtoCreate? source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var SubTotal = source.PaidPrinciple ?? 0 + source.PaidInterest ?? 0 + source.PaidCollectionFee ?? 0 + source.PaidPenaltyAmount ?? 0;

        return SubTotal;
    }
}