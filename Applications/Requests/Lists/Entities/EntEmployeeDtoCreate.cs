using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Lists.Entities;

public class EntEmployeeDtoCreate
{
    public long EmployeeId { get; set; }
    public long? StaffId { get; set; }
    public string? UserId { get; set; }
    public DateTime? Hiredate { get; set; }
    public DateTime? Enddate { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string? Gender { get; set; } = "Male";
    public string? Photo { get; set; }
    public string? AltPhone { get; set; }
    public string? MarritalStatus { get; set; }
    public string? Nationality { get; set; } = "Khmer";
    public DateTime? DateOfBirth { get; set; }
    public string? Occupation { get; set; }
    public string? Memo { get; set; }
    [ValidateComplexType]
    public EntityNameDtoCreate? EntityName { get; set; } = new EntityNameDtoCreate();
}
