namespace Applications.Requests.Lists.OtherNames;

public sealed class EmployeeConvertCreate
{
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }

    public string? MarriedStatus { get; set; } 
    public string? Photo { get; set; }
}
