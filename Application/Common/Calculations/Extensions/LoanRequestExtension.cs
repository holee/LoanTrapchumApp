using Application.Features.LoanRequests.Requests;

namespace Application.Calculations;

public static class LoanRequestExtension
{
    public static LoanRequestDtoCreate ToLoanRequestDtoCreate(this VLoanRequests req)
    {
        return new LoanRequestDtoCreate
        {
            LoanRequestId = req.LoanRequestId,
            CustomerRefId = req.CustomerId,
            RequestUserRefId = req.UserRequestId,
            LoanRequestStatusRefId = req.StatusId,
            BranchRefId = req.BranchId,
            LoanCategoryRefId = req.LoanCategoryRefId,
            BankRefId = 11,
            LoanProductRefId = req.LoanProductRefId,
            CreditAgentRefId = req.CreditAgentId,
            InstallmentName = req.InstallmentName,
            InstallmentTypeId = req.InstallmentTypeId,
            PayEvery = req.PayEvery,
            CurrencyRefId = req.CurrencyId,
            RequestDate = req.RequestDate,
            FirstPaymentDate = req.FirstPaymentDate,
            LastPaymentDate = req.LastPaymentDate,
            Installments = req.Installments,
            InstallmentType = req.InstallmentType,
            RepaymentMode = req.RepaymentMode,
            RepaymentModeRefId = req.RepaymentModeRefId,
            RequestAmount = req.RequestAmount,
            InterestRate = req.InterestRate,
            CollectionFeeModeId = req.CollectionFeeModeId,
            CollectionFeeModeName = req.CollectionFeeModeName,
            CollectionFeeRate = req.CollectionFeeRate,
            Circle = req.Circle,
            AprovedUserId = req.AprovedUserId,
            AprovedDate = req.AprovedDate,
            Summiter = req.Summiter,
            AdminFeeRate = req.AdminFeeRate,
            AdminFeeAmount = req.AdminFeeAmount,
            Remark = req.Remark,
            Memo = req.Memo,
            FullName = req.AltName,
            LoanPurpose = req.LoanPurpose,
        };
    }

    public static LoanDtoCreate ToLoanDtoCreate(this VLoanRequests req)
    {
        return new LoanDtoCreate
        {
            LoanRequestId = req.LoanRequestId,
            CustomerRefId = req.CustomerId,
            AltName = req.AltName,
            BranchId = req.BranchId,
            LoanCategoryRefId = req.LoanCategoryRefId,
            LoanCategoryName = req.LoanCategoryName,
            BankRefId = req.BankId,
            LoanProductRefId = req.LoanProductRefId,
            CreditAgentRefId = req.CreditAgentId,
            CreditAgentName = req.Summiter,
            InstallmentName = req.InstallmentName,
            InstallmentTypeId = req.InstallmentTypeId,
            PayEvery = req.PayEvery,
            CurrencyId = req.CurrencyId,
            CurrencyName = req.CurrencyName,
            StartDate = req.RequestDate,
            RegisterDate = req.RequestDate,
            FirstPaymentDate = req.FirstPaymentDate,
            LastPaymentDate = req.LastPaymentDate,
            Installments = req.Installments,
            Period = req.Installments,
            InstallmentType = req.InstallmentType,
            RepaymentModeName = req.RepaymentMode,
            RepaymentModeRefId = req.RepaymentModeRefId,
            Principle = req.AprovedAmount,
            InterestRate = req.InterestRate,
            CollectionFeeModeId = req.CollectionFeeModeId,
            OtherChargName = req.CollectionFeeModeName,
            CollectionFeeRate = req.CollectionFeeRate,
            Circle = req.Circle,
            AdminFeeRate = req.AdminFeeRate,
            AdminFeeAmount = req.AdminFeeAmount,
            Remark = req.Remark,
            BranchName = req.BranchName,
            AprovedUserId = req.AprovedUserId,
            ApprovedBy = req.AprovedBy,
        };
    }



}
