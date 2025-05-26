using Domains.Models.Lists;
using Domains.Models.Loans;

namespace Applications.Requests.Loans.LoanRequests;

public sealed class LoanRequestGuarantorsDtoCreate 
{
    public string? LoanRequestId { get; set; } 
    public string? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanGuarantorLink> GuarantorsLink { get; set; } = []; 

}
