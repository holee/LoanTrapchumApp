using Applications.Features.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Features.Loans.Mappers;

public static class LoanDtoCreateMapper 
{
    public static LoanDisbursment MapLoanCreate(this LoanDtoCreate dto)
    {
        return new LoanDisbursment
        {
            LoanId = dto.LoanId ?? $"dis-{Guid.CreateVersion7()}",
            LoanRequestId = dto.LoanRequestId,
            LoanStatusRefId = dto.LoanStatusRefId,
            LoanSubTypeRefId = dto.LoanSubTypeRefId,
            LoanCategoryRefId = dto.LoanCategoryRefId,
            BankRefId = dto.BankRefId,
            BranchRefId = dto.BranchId,
            Installments = dto.Installments,
            UserRefId = dto.UserRefId,
            Principle = dto.Principle,
            CreditAgentRefId = dto.CreditAgentRefId,
            LoanProductRefId = dto.LoanProductRefId,
            LoanAccountCode = dto.LoanAccountCode,
            Period = dto.Period,
            PeriodName = dto.PeriodName,
            CurrencyRefId = dto.CurrencyRefId,
            PenaltyRate = dto.PenaltyRate,
            StartDate = dto.StartDate?.UtcDateTime,
            FirstPaymentDate = dto.FirstPaymentDate?.UtcDateTime,
            LastPaymentDate = dto.LastPaymentDate?.UtcDateTime,
            TermTypeId = dto.TermTypeId,
            InstallmentTypeId = dto.InstallmentTypeId,
            CustomerRefId = dto.CustomerRefId,
            RegisterDate = dto.RegisterDate?.UtcDateTime ?? DateTime.UtcNow,
            PayEvery = dto.PayEvery,
            AdminFeeRate = dto.AdminFeeRate,
            AdminFeeAmount = dto.AdminFeeAmount,
            InstallmentType = dto.InstallmentType,
            CollectionFeeRate = dto.CollectionFeeRate,
            Remark = dto.Remark,
            BranchName = dto.BranchName,
            Circle = dto.Circle,
            RepaymentModeRefId = dto.RepaymentModeRefId,
            InterestRate = dto.InterestRate,
            LoanSchedules = dto.LoanSchedules?.MapScedules()
        };
    }

    
    public static LoanSchedule MapSchedule(this LoanScheduleDtoCreate s)
    {
        return new LoanSchedule
        {
            InstallmentNo = s.InstallmentNo,
            AgentRefId = s.AgentRefId,
            Interest = s.Interest,
            BranchId = s.BranchId,
            Principal = s.Principal,
            CollectionFee = s.CollectionFee,
            PaymentDate = s.PaymentDate,
            ScheduleStatusRefId = s.ScheduleStatusRefId,
            TotalDay = s.TotalDay,
            TotalPay = s.TotalPay,
            ScheduleNum = s.ScheduleNum,
            LoanOutstanding = s.LoanOutstanding
        };
    }


    public static List<LoanSchedule> MapScedules(this IEnumerable<LoanScheduleDtoCreate> dto)
    {
        return dto.Select(s => new LoanSchedule
        {
            InstallmentNo = s.InstallmentNo,
            AgentRefId = s.AgentRefId,
            Interest = s.Interest,
            BranchId = s.BranchId,
            Principal = s.Principal,
            CollectionFee = s.CollectionFee,
            PaymentDate = s.PaymentDate,
            ScheduleNum = s.ScheduleNum,
            ScheduleStatusRefId =  s.ScheduleStatusRefId,
            TotalDay = s.TotalDay, 
            TotalPay = s.TotalPay,
            LoanOutstanding = s.LoanOutstanding
        }).ToList();
        

    }
}


