using Applications.Features.Entities;
using Applications.Features.Entities;
using Domains.Models.Loans;

namespace Applications.Responses.Lists.Entities;
public class EntEmployeeDto
{
    public long EmployeeId { get; set; }
    public long? StaffId { get; set; }
    public string? UserId { get; set; }
    public DateTime? Hiredate { get; set; }
    public DateTime? Enddate { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string? Gender { get; set; }
    public string? Photo { get; set; }
    public string? AltPhone { get; set; }
    public string? MarritalStatus { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DepartmentDto? Department { get; set; }
    public BranchDto? Branch { get; set; }
    public string? Occupation { get; set; }
    public string? Memo { get; set; }
    public LoanCreditAgent? LoanCreditAgent { get; set; }
    public EntityNameDto? EntityName { get; set; }
}
