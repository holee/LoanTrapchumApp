using Application.Features.LoanRequests.Requests;
using Application.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Application.Features.Customers.Extensions;

public static class LoanDocumentsExtensions
{
    public static LoanRequestDocuments ToLoanRequestDocuments(string? customerId, string? loanRequestId, int? branchId, IEnumerable<VLoanRequestsDocuments> doc)
    {
        return new LoanRequestDocuments
        {
            CustomerRefId = customerId,
            BranchRefId = branchId,
            LoanRequestId = loanRequestId,
        };
    }

    public static List<LoanDocumentDtoCreate> ToLoanDocumentsCreate(string? customerId, string? loanRequestId, int? branchId, IEnumerable<VLoanRequestsDocuments> doc)
    {
        return doc.Select(x => new LoanDocumentDtoCreate
        {
            BranchId = branchId,
            LoanRequestId = loanRequestId,
            OriginalName = x.OriginalName,
            NewFileName = x.NewFileName,
            FileUrl = x.FileUrl,
            Description = x.Description,
            DocumentId= x.DocumentId

        }).ToList();
    }

    public static List<LoanDocumentDtoCreate> ToLoanDocumentsEdit(this IEnumerable<VLoanRequestsDocuments> doc)
    {
        return doc.Select(x => new LoanDocumentDtoCreate
        {
            Id = x.Id,
            BranchId = x.BranchRefId,
            LoanRequestId = x.LoanRequestId,
            OriginalName = x.OriginalName,
            NewFileName = x.NewFileName,
            FileUrl = x.FileUrl,
            FileSize = x.FileSize,
            Description = x.Description,
            DocumentId = x.DocumentId,
        }).ToList();
    }

}