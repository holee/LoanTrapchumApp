using Applications.Requests.Loans;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Responses;

public static class LoanDocumentsExtensions
{
    public static LoanRequestDocuments ToLoanRequestDocuments(Guid? customerId, Guid? loanRequestId, int? branchId, IEnumerable<VLoanRequestsDocuments> doc)
    {
        return new LoanRequestDocuments
        {
            CustomerRefId = customerId,
            BranchRefId = branchId,
            LoanRequestId = loanRequestId,
            // LoanDocuments = doc.Select(x => new LoanDocumentDtoCreate
            // {
            //     BranchId = branchId,
            //     LoanRequestId = loanRequestId,
            //     OriginalName = x.OriginalName,
            //     NewFileName = x.NewFileName,
            //     FileUrl = x.FileUrl,
            //     Description = x.Description
            // }).ToList(),
        };
    }
}