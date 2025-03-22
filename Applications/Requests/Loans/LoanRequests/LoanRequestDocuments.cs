using Domains.Models.Loans;

namespace Applications.Requests.Loans.LoanRequests;

public sealed class LoanRequestDocumentsDtoCreate 
{
    public long LoanRequestId { get; set; }
    public long? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanDocumentDtoCreate> LoanDocuments { get; set; } = [];

}