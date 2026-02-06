using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Departments.Dto; 

public class DepartmentDtoCreate
{

    [Required(AllowEmptyStrings = false)]
    public string DepartmentName { get; set; } = default!;
    public int? BranchId { get; set; }
    public string? Description { get; set; } 
}
 