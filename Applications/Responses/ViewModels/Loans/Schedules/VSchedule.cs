using Applications.Features.LoanPayments.Requests;
using Applications.Features.Schedules.Dto;
using Applications.Responses.Paginations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Loans.Schedules;

public sealed class ScheduleVms
{
    public int Count { get; set; } = 0;
    public Pager<VSchedule> Schedules { get; set; }
}

public sealed class VSchedule
{
    public long? LoanScheduleId { get; set; }
    public string? CreditAgentId { get; set; }
    public string? CreditAgentNameKh { get; set; }
    public string? CreditAgentNameEn { get; set; }
    public string? CustomerId { get; set; }
    public string? FullName { get; set; }
    public string? AltName { get; set; }
    public string? LoanId { get; set; }
    public int? BranchId { get; set; }
    public int? CurrencyId { get; set; }
    public int? InstallmentNo { get; set; }
    public decimal? InterestRate { get; set; } 
    public DateTime? PaymentDate { get; set; }
    public int? TotalDay { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFee { get; set; }
    public decimal? PenaltyRate { get; set; }
    public int? DayArrea { get; set; } = 0;
    public decimal? PenaltyAmount { get; set; } = 0;
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? PaidInterest { get; set; }
    public decimal? PaidCollectionFee { get; set; }
    public decimal? PaidAmount { get; set; } = 0;
    public decimal? LeftInterestAmount { get; set; } = 0;
    public decimal? LeftCollectionFeeAmount { get; set; } = 0;
    public decimal? LeftPrinciple { get; set; } = 0;
    public decimal? LeftAmount { get; set; } = 0;
    public decimal? TotalFee { get; set; } = 0;
    public int? ScheduleStatusId { get; set; }
    public string? ScheduleStatus { get; set; }
    public bool IsLate { get; set; } = false;
    public int? ScheduleNum { get; set; } = 1;
    [NotMapped] public bool Selected { get; set; } = false;
    [NotMapped] public bool IsPaid => ScheduleStatusId == 1;
    [NotMapped] public bool IsPartialPaid => ScheduleStatusId == 4;
    [NotMapped] public List<PaymentLineDetailsDto> PaymentLines { get; set; } = [];

    public LoanPaymentLineDtoCreate MapToLoanPaymentLine()
    {
        throw new NotImplementedException();
    }
}
