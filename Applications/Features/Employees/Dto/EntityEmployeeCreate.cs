using Applications.Features.OtherNames;

namespace Applications.Features.Employees.Dto;

public sealed class EntityEmployeeCreate :BaseEntityNamesCreate
{
    public string? EmployeeId { get; set; }
    public string? StaffId { get; set; }
    public string? UserRefId { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string? Photo { get; set; }
}
