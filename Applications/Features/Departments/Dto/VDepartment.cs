namespace Applications.Features.Departments.Dto;

public sealed class VDepartment
{
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool Used { get; set; } = false;
}
