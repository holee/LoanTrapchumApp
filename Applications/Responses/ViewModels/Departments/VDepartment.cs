namespace Applications.Responses.ViewModels.Departments;

public class VDepartment
{
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int? EditSeq { get; set; } = 0;
    public bool Used { get; set; } = false;
}
