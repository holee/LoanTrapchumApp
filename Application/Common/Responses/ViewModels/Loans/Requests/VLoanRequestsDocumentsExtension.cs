using Application.Features.LoanRequests.Requests;

namespace Application.Responses.ViewModels.Loans.Requests;

public static class VLoanRequestsDocumentsExtension
{
    public static List<LoanDocumentDtoCreate> LoanDocumentDtoCreate(this List<VLoanRequestsDocuments> docs, int? branchId, string? loanRequestId)
    {
        return docs?.Select(f => new LoanDocumentDtoCreate
        {
            Id = f.Id,
            LoanRequestId = loanRequestId,
            BranchId = branchId,
            OriginalName = f.OriginalName,
            NewFileName = f.NewFileName,
            FileUrl = f.FileUrl,
            Description = f.Description,

        }).ToList() ?? [];
    }
    public static List<LoanDocumentDtoCreate> LoanDocumentDtoCreate(this List<VLoanRequestsDocuments> docs)
    {
        return docs?.Select(f => new LoanDocumentDtoCreate
        {
            Id = f.Id,
            LoanRequestId = f.LoanRequestId,
            BranchId = f.BranchRefId,
            OriginalName = f.OriginalName,
            NewFileName = f.NewFileName,
            FileUrl = f.FileUrl,
            Description = f.Description,

        }).ToList() ?? [];
    }
}