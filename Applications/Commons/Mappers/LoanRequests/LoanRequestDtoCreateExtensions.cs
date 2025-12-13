using Applications.Features.Collaterals.Dtos;
using Applications.Features.Entities.Guarantors;
using Applications.Features.LoanRequests.Command.CreateLoanRequest;
using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
using Applications.Features.LoanRequests.Command.SaveLoanRequest;
using Applications.Features.LoanRequests.Requests;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Collaterals;
using Domains.Models.Lists;
using Domains.Models.Loans;

namespace Applications.Commons.Mappers.LoanRequests;

public static class LoanRequestDtoCreateExtensions
{
    private static LoanDocuments MapToLoanDocument(this LoanDocumentDtoCreate req)
    {
        return new LoanDocuments
        {
            BranchId = req.BranchId,
            LoanRequestId = req.LoanRequestId,
            OriginalName = req.OriginalName,
            NewFileName = req.NewFileName
        };
    }

    private static LoanDocuments MapToLoanDocument(this LoanDocumentDtoUpdate req)
    {
        return new LoanDocuments
        {
            Id = req.Id,
            BranchId = req.BranchId,
            LoanRequestId = req.LoanRequestId,
            OriginalName = req.OriginalName,
            NewFileName = req.NewFileName
        };
    }

    private static LoanDescriptions MapToLoanDescription(this LoanDescriptionsDtoCreate req)
    {
        return new LoanDescriptions
        {
            Memo = req.Memo,
            UserRefId = req.UserRefId,
            LoanRequestRefId = req.LoanRequestRefId
        };
    }

    private static LoanDescriptions MapToLoanDescription(this LoanDescriptionsDtoUpdate req)
    {
        return new LoanDescriptions
        {
            Id = req.Id,
            Memo = req.Memo,
            LoanRequestRefId = req.LoanRequestRefId
        };
    }

    private static LoanGuarantorLink MapToLoanGuarantorLink(this LoanGuarantorDtoCreate req)
    {
        return new LoanGuarantorLink
        {
            BranchId = req.BranchId,
            LoanRequestRefId = req.LoanRequestRefId,
            EntityNameRefId = req.EntityNameRefId
        };
    }


    private static CollateralLink MapToCollateralLinesLink(this CollateralLinkDtoCreate req)
    {
        return new CollateralLink
        {
            BranchId = req.BranchId,
            CollateralLineRefId = req.CollateralLineRefId,
            LoanRequestRefId = req.LoanRequestRefId,
            CollateralRefId = req.CollateralRefId,
            Description = req.Description
        };
    }


    public static LoanRequest MapToLoanCreateNew(this LoanRequestDtoCreate req)
    {
        return new LoanRequest
        {
            LoanRequestId = req.LoanRequestId ?? $"req-{Guid.CreateVersion7()}",
            CustomerRefId = req.CustomerRefId,
            RequestUserRefId = req.RequestUserRefId,
            CreditAgentRefId = req.CreditAgentRefId,
            AprovedUserId = req.AprovedUserId,
            LoanRequestStatusRefId = req.LoanRequestStatusRefId,
            BranchRefId = req.BranchRefId,
            RepaymentModeRefId = req.RepaymentModeRefId,
            LoanSubTypeRefId = req.LoanSubTypeRefId,
            LoanCategoryRefId = req.LoanCategoryRefId,
            BankRefId = req.BankRefId,
            LoanProductRefId = req.LoanProductRefId,
            InstallmentName = req.InstallmentName,
            InstallmentTypeId = req.InstallmentTypeId ?? 0,
            PayEvery = req.PayEvery ?? 1,
            CurrencyRefId = req.CurrencyRefId,
            RequestDate = req.RequestDate,
            FirstPaymentDate = req.FirstPaymentDate ?? DateTime.Now.AddMonths(1),
            LastPaymentDate = req.LastPaymentDate,
            Installments = req.Installments ?? 12,
            InstallmentType = req.InstallmentType,
            RepaymentMode = req.RepaymentMode,
            RequestAmount = req.RequestAmount,
            AprovedAmount = req.AprovedAmount,
            InterestRate = req.InterestRate,
            CollectionFeeRate = req.CollectionFeeRate,
            Circle = req.Circle ?? 1,
            AprovedDate = req.AprovedDate,
            AdminFeeRate = req.AdminFeeRate,
            AdminFeeAmount = req.AdminFeeAmount,
            PenaltyRate = req.PenaltyRate,
            Remark = req.Remark,
            CollectionFeeModeId = req.CollectionFeeModeId ?? 1,
            CollectionFeeModeName = req.CollectionFeeModeName ?? string.Empty,
            LoanDocuments = req.LoanDocuments.Select(d => d.MapToLoanDocument()).ToList() ?? [],
            GuarantorsLink = req.GuarantorsLink.Select(g => g.MapToLoanGuarantorLink()).ToList() ?? [],
            CollateralLinesLink = req.CollateralLinesLink.Select(c => c.MapToCollateralLinesLink()).ToList() ?? [],
            LoanDescriptions = req.LoanDescriptions.Select(ld => ld.MapToLoanDescription()).ToList() ?? []
        };
    }

    public static LoanRequest MapToLoanCreateNew(this CreateLoanRequestCommand req)
    {
        return req.Request.MapToLoanCreateNew();
    }

    public static LoanRequest MapToLoanRequestSave(this LoanRequestDtoCreate req)
    {
        return new LoanRequest
        {
            LoanRequestId = req.LoanRequestId ?? $"req-{Guid.CreateVersion7()}",
            CustomerRefId = req.CustomerRefId,
            RequestUserRefId = req.RequestUserRefId,
            CreditAgentRefId = req.CreditAgentRefId,
            AprovedUserId = req.AprovedUserId,
            LoanRequestStatusRefId = req.LoanRequestStatusRefId,
            BranchRefId = req.BranchRefId,
            RepaymentModeRefId = req.RepaymentModeRefId,
            LoanSubTypeRefId = req.LoanSubTypeRefId,
            LoanCategoryRefId = req.LoanCategoryRefId,
            BankRefId = req.BankRefId,
            LoanProductRefId = req.LoanProductRefId,
            InstallmentName = req.InstallmentName,
            InstallmentTypeId = req.InstallmentTypeId ?? 0,
            PayEvery = req.PayEvery ?? 1,
            CurrencyRefId = req.CurrencyRefId,
            RequestDate = req.RequestDate,
            FirstPaymentDate = req.FirstPaymentDate ?? DateTime.Now.AddMonths(1),
            LastPaymentDate = req.LastPaymentDate,
            Installments = req.Installments ?? 12,
            InstallmentType = req.InstallmentType,
            RepaymentMode = req.RepaymentMode,
            RequestAmount = req.RequestAmount,
            AprovedAmount = req.AprovedAmount,
            InterestRate = req.InterestRate,
            CollectionFeeRate = req.CollectionFeeRate,
            Circle = req.Circle ?? 1,
            AprovedDate = req.AprovedDate,
            AdminFeeRate = req.AdminFeeRate,
            AdminFeeAmount = req.AdminFeeAmount,
            PenaltyRate = req.PenaltyRate,
            Remark = req.Remark,
            CollectionFeeModeId = req.CollectionFeeModeId ?? 1,
            CollectionFeeModeName = req.CollectionFeeModeName ?? string.Empty,
            LoanDescriptions = req.LoanDescriptions.Select(ld => ld.MapToLoanDescription()).ToList() ?? []
        };
    }

    public static LoanRequest MapToLoanRequestSave(this SaveLoanRequestCommand req)
    {
        return req.Request.MapToLoanRequestSave();
    }

    public static LoanRequest MapToLoanRequestSave(this SaveEditLoanRequestCommand req)
    {
        return req.Request.MapToLoanRequestSave();
    }



    public static AproveLoanDtoCreate ApprovedLoanRequestDtoCreate(this ApprovedRequest req)
    {
        return new AproveLoanDtoCreate
        {
            LoanRequestId = req.LoanRequestId,
            CustomerId = req.CustomerId,
            BranchId = req.BranchId,
            AdminFeeAmount = req.AdminFeeAmount,
            AprovedAmount = req.AprovedAmount,
            AprovedDate = DateTime.Now,
        };
    }



}