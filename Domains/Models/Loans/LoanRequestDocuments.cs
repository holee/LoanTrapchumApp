
namespace Domains.Models.Loans;

public sealed class LoanRequestDocuments
{
    public long LoanRequestId { get; set; }
    public long? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanDocument> LoanDocuments { get; set; } = [];

}