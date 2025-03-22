
namespace Domains.Models.Loans;

public sealed class LoanRequestGuarantors 
{
    public Guid? LoanRequestId { get; set; } 
    public Guid? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanGuarantor> GuarantorsLink { get; set; } = []; 

}
