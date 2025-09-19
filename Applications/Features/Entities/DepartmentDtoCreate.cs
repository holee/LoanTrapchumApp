using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Entities; 

public class DepartmentDtoCreate
{

    [Required(AllowEmptyStrings = false)]
    public string DepartmentName { get; set; } = default!;
    public string? Description { get; set; } 
}
 