namespace Applications.Responses.ViewModels.Employees;

public class VEmployeeList
{
    public long EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; } 
    public string? Phone { get; set; }
    public string? BranchName { get; set; }
    public string? DepartmentName { get; set; }
    public string? Occupation { get; set; }
    public bool IsActive { get; set; }
    public string? Address { get; set; } 
}