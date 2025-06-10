using Applications.Responses.ViewModels.Loans.Payments;
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
            BranchName = s.LoanBranchName,
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
