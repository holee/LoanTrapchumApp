
namespace Domains.Models.Loans;

public sealed class LoanRequestDocuments
{
    public string? LoanRequestId { get; set; }
    public string? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanDocuments> LoanDocuments { get; set; } = [];

}