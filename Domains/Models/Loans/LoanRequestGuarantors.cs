
namespace Domains.Models.Loans;

public sealed class LoanRequestGuarantors 
{
    public string? LoanRequestId { get; set; } 
    public string? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanGuarantor> GuarantorsLink { get; set; } = []; 

}
