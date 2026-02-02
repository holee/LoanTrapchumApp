namespace Applications.Features.Departments.Dto;

public class DepartmentDtoUpdate : DepartmentDtoCreate
{
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; } = false;
    public int EditSeq { get; set; } = 0; 
}
