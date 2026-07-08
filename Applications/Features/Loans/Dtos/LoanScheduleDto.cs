using Applications.Features.LoanPayments.Requests;
using Applications.Features.Schedules.Dto;
using Applications.Responses.Paginations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Loans.Dtos;

public sealed class LoanScheduleDto 
{
    public long? LoanScheduleId { get; set; }
    public string? LoanId { get; set; }
    public int? BranchId { get; set; }
    public int? CurrencyId { get; set; }
    public int? InstallmentNo { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? TotalDay { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Interest { get; set; }
    public decimal? CollectionFee { get; set; }
    public decimal? TotalFee { get; set; } = 0;
    public decimal? TotalPay { get; set; }
    public decimal? LoanOutstanding { get; set; }
}
