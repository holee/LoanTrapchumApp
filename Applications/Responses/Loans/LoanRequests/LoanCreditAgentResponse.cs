using Applications.Responses.Lists.Entities;
namespace Applications.Responses.Loans.LoanRequests;

public sealed class LoanCreditAgentResponse
{
    public int LoanCreditAgentId { get; set; }
    public long? EmployeeRefId { get; set; } 
    public int? CreditAssistant { get; set; }
    public int? SubManager { get; set; }
    public int? Manager { get; set; }
    public int? SubBranch { get; set; }
    public int? HeadOffice { get; set; }
    public bool? IsRemove { get; set; }
    public EntEmployeeDto? Employee { get; set; }
}
