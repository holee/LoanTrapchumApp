using Application.Enumerations;
using Application.Extensions.Enums;
using Application.Features.Loans.Dtos.Requests;
using Domains.Enumerations;
using Domains.Models.Loans;
using Domains.Models.Transaction;

namespace Application.Features.Loans.Mappers;

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
            LoanProductRefId = dto.LoanProductRefId ?? (int)ProductTypeOption.Mortgage,
            LoanAccountCode = dto.LoanAccountCode,
            Period = dto.Period,
            PeriodName = dto.PeriodName,
            CurrencyRefId = dto.CurrencyId,
            PenaltyRate = dto.PenaltyRate,
            StartDate = dto.StartDate,
            FirstPaymentDate = dto.FirstPaymentDate,
            LastPaymentDate = dto.LastPaymentDate,
            TermTypeId = dto.TermTypeId,
            InstallmentTypeId = dto.InstallmentTypeId,
            CustomerRefId = dto.CustomerRefId,
            RegisterDate = dto.RegisterDate ?? DateTime.UtcNow,
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
            LoanSchedules = dto.LoanSchedules?.MapScedules(),
            LoanTransaction = new LoanTransaction
            {
                TransactionsId = $"trx-{Guid.CreateVersion7()}",
                BranchRefId = dto.BranchId,
                TransactionDate = DateTime.UtcNow,
                TransactionTypeName = TransactionTypes.LD.Description(),
                TranTypeRefId = (int)TransactionTypes.LD,
                UserRefId = dto.UserRefId
            }
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
            ScheduleStatusRefId = s.ScheduleStatusRefId,
            TotalDay = s.TotalDay,
            TotalPay = s.TotalPay,
            LoanOutstanding = s.LoanOutstanding
        }).ToList();


    }
}


