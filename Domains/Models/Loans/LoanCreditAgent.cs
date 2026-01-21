using Domains.Models.Bases;
using Domains.Models.Lists.Employees;

namespace Domains.Models.Loans;

public sealed class LoanCreditAgent : BaseEntity
{
    public string? EmployeeRefId { get; set; }
    public int? CreditAssistant { get; set; }
    public int? SubManager { get; set; }
    public int? Manager { get; set; }
    public int? SubBranch { get; set; }
    public int? HeadOffice { get; set; }
    public bool? IsRemove { get; set; } = false;
    public Employee? Employee { get; set; }
}
