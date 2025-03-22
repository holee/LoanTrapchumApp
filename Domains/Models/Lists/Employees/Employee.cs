using Domains.Models.Bases;
using Domains.Models.Loans;

namespace Domains.Models.Lists.Employees;

public class Employee : BaseAddress
{
    public long EmployeeId { get; set; }
    public long? StaffId { get; set; }
    public string? UserRefId { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }
    public string? AltName { get; set; }
    public string? EntType { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DepartmentRefId { get; set; }
    public string? JobTitle { get; set; }
    public string? Gender { get; set; }
    public string? Salutation { get; set; }
    public string? Occupation { get; set; }
    public int? BranchRefId { get; set; }
    public string? MarritalStatus { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Photo { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? IdCard { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }
    public LoanCreditAgent? LoanCreditAgent { get; set; }
    public Branch? Branch { get; set; }
    public Department? Department { get; set; }
    public void AddCreditAgent(LoanCreditAgent creditAgent)
    {
        this.LoanCreditAgent = creditAgent;
    }
}
