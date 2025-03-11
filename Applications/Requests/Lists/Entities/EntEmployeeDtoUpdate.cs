namespace Applications.Requests.Lists.Entities;

public class EntEmployeeDtoUpdate
{
    public long EmployeeId { get; set; }
    public long? StaffId { get; set; }
    public string? UserId { get; set; }
    public DateTime? Hiredate { get; set; }
    public DateTime? Enddate { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string? Gender { get; set; } = string.Empty;
    public string? Photo { get; set; } = string.Empty;
    public string? AltPhone { get; set; } = string.Empty;
    public string? MarritalStatus { get; set; } = string.Empty;
    public string? Nationality { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Occupation { get; set; } = string.Empty;
    public string? Memo { get; set; } = string.Empty;
    public EntityNameDtoUpdate? EntityName { get; set; } = new();

}
