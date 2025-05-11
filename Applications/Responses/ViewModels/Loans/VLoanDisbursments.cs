using System.ComponentModel.DataAnnotations.Schema;

using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Responses.ViewModels.Loans;



public static class VLoanDisbursmentExtensions
{
    public static LoanScheduleVm ToLoanScheduleVm(this VLoanDisbursments s)
    {
        var vm = new LoanScheduleVm 
        {
            LoanId = s.LoanId ?? string.Empty,
            ContractNumber = s.ContractNumber,
            BranchName = s.BranchName,
            FullName = s.CustomerName,
            CustomerKhmerName = s.AltName,
            CustomerPhone = s.Phone,
            Salutation = s.Salutation,
            RegisterDate = s.RegisterDate,
            AgentName = s.AgentName,
            AgentKhmerName = string.Empty,
            AgentPhone = s.AgentPhone,
            CurrencyName = s.CurrencyName,
            Principle = s.Amount,
            InterestRate = s.InterestRate,
            AdminFeeAmount = s.AdminFeeAmount,
            AdminFeeRate = s.AdminFeeRate,
            OutStanding = s.OutStanding,
            FirstPaymentDate = s.FirstPaymentDate,
            LastPaymentDate = s.LastPaymentDate,
            Address = s.Address,
            Circle = s.Circle,
            Period = s.Period,
            CurrencyId=s.CurrencyId,
            PeriodName = s.PeriodName,
            InstallmentType = s.InstallmentType,
            LoanSchedules = s.Schedules!.Select(vs => new ScheduleViewModel
            {
                LoanScheduleId = vs.LoanScheduleId,
                BranchId = vs.BranchId,
                CurrencyId = vs.CurrencyId,
                InstallmentNo = vs.InstallmentNo,
                PaymentDate = vs.PaymentDate,
                TotalDay = vs.TotalDay,
                Principal = vs.Principle,
                Interest = vs.Interest,
                CollectionFee = vs.CollectionFee,
                TotalPay = vs.TotalPay,
                LoanOutstanding = vs.LoanOutstanding,
            }).ToList()
        };

        return vm;
    }
}

public sealed class VLoanDisbursments
{
    public string? LoanId { get; set; }
    public string? ContractNumber { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? CustomerId { get; set; }
    public string? AltName { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Salutation { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? CreditAgentRefId { get; set; }
    public string? AgentName { get; set; }
    public string? AgentPhone { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? Principle { get; set; } 
    public decimal? Amount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? OutStanding { get; set; }
    public decimal? TotalInterest { get; set; }
    public decimal? TotalCollectionFee { get; set; }
    public decimal? TotalOtherCharge { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? LoanStatus { get; set; }
    public int LoanstatusId { get; set; }
    public string? Address { get; set; }
    public int? Circle { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public string? InstallmentType { get; set; }
    [NotMapped]
    public ICollection<VSchedule>? Schedules { get; set; } = [];
}
