namespace Applications.Features.LoanRequests.Requests;

public sealed class LoanRequestDocumentsDtoCreate 
{
    public long LoanRequestId { get; set; }
    public long? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanDocumentDtoCreate> LoanDocuments { get; set; } = [];

}