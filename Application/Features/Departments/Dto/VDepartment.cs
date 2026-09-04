using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Departments.Dto;

public sealed class VDepartment
{
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool Used { get; set; } = false;
    public DateTimeOffset? ModifiedDT { get; set; }
    [NotMapped]public int RowIndex { get; set; } = 0;
}
