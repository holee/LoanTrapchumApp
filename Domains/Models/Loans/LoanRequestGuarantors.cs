
namespace Domains.Models.Loans;

public sealed class LoanRequestGuarantors 
{
    public long LoanRequestId { get; set; } 
    public long? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanGuarantor> GuarantorsLink { get; set; } = []; 

}
