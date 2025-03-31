namespace Applications.Requests.Lists.Employees;

public sealed class EmployeeCreate
{
    public Guid? EmployeeId { get; set; }
    public long? StaffId { get; set; }
    public string? UserRefId { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string? MarritalStatus { get; set; }
    public string? Photo { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
}
