
namespace Domains.Models.Loans;

public sealed class LoanRequestDocuments
{
    public Guid? LoanRequestId { get; set; }
    public Guid? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanDocument> LoanDocuments { get; set; } = [];

}