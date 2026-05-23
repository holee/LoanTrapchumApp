using Domains.Models.Loans;

namespace Domains.Models.Lists.Employees;

public sealed class Employee
{
    public string? EmployeeId { get; set; }
    public string? StaffId { get; set; }
    public string? UserRefId { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DepartmentRefId { get; set; }   
    public int? BranchRefId { get; set; }
    public string? Photo { get; set; }
    public LoanCreditAgent? LoanCreditAgent { get; set; }
    public Branch? Branch { get; set; }
    public Department? Department { get; set; }
    public EntityNames? EntityNames { get; set; }
    public EmergencyContact? Contact { get; set; }
}