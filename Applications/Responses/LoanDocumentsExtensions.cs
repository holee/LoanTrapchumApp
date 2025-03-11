using Applications.Requests.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Responses;

public static class LoanDocumentsExtensions
{
    public static LoanRequestDocuments ToLoanRequestDocuments(long customerId, long loanRequestId, int branchId, IEnumerable<VLoanRequestsDocuments> doc)
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