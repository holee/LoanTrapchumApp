using Domains.Models.Lists;

namespace Applications.Features.LoanRequests.Requests;

public sealed class LoanRequestGuarantorsDtoCreate 
{
    public string? LoanRequestId { get; set; } 
    public string? CustomerRefId { get; set; }
    public int? BranchRefId { get; set; }
    public List<LoanGuarantorLink> GuarantorsLink { get; set; } = []; 

}
