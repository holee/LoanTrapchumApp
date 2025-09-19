namespace Applications.Features.Entities;

public class EmployeeConvertDto : EntityNameConvertDto 
{

    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DepartmentRefId { get; set; }
    public string? Photo { get; set; }
    public int? OccupationRefId { get; set; }
    public string? AltPhone { get; set; }
    public string? Email { get; set; }
}
