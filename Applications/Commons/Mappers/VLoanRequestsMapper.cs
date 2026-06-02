using Applications.Features.LoanRequests.Requests;
using Applications.Responses.ViewModels.Loans.LoanRequests;

namespace Applications.Commons.Mappers;

public static class VLoanRequestsMapper
{
    public static List<VLoanRequestGuarantor> ToLoanRequestGuarantors(this VLoanRequests req)
    {
        return req.Guarantors.ToList() ?? [];
    }

    public static List<VLoanRequestCollateralLines> ToLoanRequestCollaterals(this VLoanRequests req)
    {
        return req.Collaterals.Select(x => new VLoanRequestCollateralLines
        {
            Id = x.Id,
            BranchId = x.BranchId,
            CollateralId = x.CollateralId,
            CollateralLineId = x.CollateralLineId,
            CollateralNo = x.CollateralNo,
            InUsed = x.InUsed,
            Location = x.Location,
            MarketPrice = x.MarketPrice,
            OwnerName = x.OwnerName,
            PawnPrice = x.PawnPrice,
            Quantity = x.Quantity,
            HotPrice = x.HotPrice,
            ReferenceNo = x.ReferenceNo,
            SubTypeName = x.SubTypeName,
            TypeName = x.TypeName,
        }).ToList() ?? [];
    }

    public static List<VLoanRequestsDocuments> ToLoanRequestDocuments(this VLoanRequests req)
    {
        return req.Documents.ToList() ?? [];
    }

    public static List<VLoanRequestDescription> ToLoanRequestDecriptions(this VLoanRequests req)
    {
        return req.Descriptions.ToList() ?? [];
    }

    public static LoanRequestDtoCreate MapToLoanRequestCreate(this VLoanRequests source)
    {
        if (source == null)
        {
            return new();
        }

        return new LoanRequestDtoCreate
        {
            LoanRequestId = source.LoanRequestId,
            CustomerRefId = source.CustomerId,
            FullName = source.FullName,
            LoanRequestStatusRefId = source.StatusId,
            BranchRefId = source.BranchId,
            RequestUserRefId = source.UserRequestId,
            RepaymentModeRefId = source.RepaymentModeRefId,
            LoanCategoryRefId = source.LoanCategoryRefId,
            BankRefId = source.BankId,
            LoanProductRefId = source.LoanProductRefId,
            CreditAgentRefId = source.CreditAgentId,
            InstallmentName = source.InstallmentName,
            InstallmentTypeId = source.InstallmentTypeId,
            PayEvery = source.PayEvery,
            CurrencyRefId = source.CurrencyId,
            RequestDate = source.RequestDate,
            FirstPaymentDate = source.FirstPaymentDate,
            LastPaymentDate = source.LastPaymentDate,
            Installments = source.Installments,
            InstallmentType = source.InstallmentType,
            RepaymentMode = source.RepaymentMode,
            RequestAmount = source.RequestAmount,
            AprovedAmount = source.AprovedAmount,
            InterestRate = source.InterestRate,
            CollectionFeeModeId = source.CollectionFeeModeId,
            CollectionFeeModeName = source.CollectionFeeModeName,
            CollectionFeeRate = source.CollectionFeeRate,
            Circle = source.Circle,
            AprovedUserId = source.AprovedUserId,
            AprovedDate = source.AprovedDate,
            ApprovedBy = source.AprovedBy,
            Summiter = source.Summiter,
            AdminFeeRate = source.AdminFeeRate,
            AdminFeeAmount = source.AdminFeeAmount,
            Remark = source.Remark,
        };
    }



}
