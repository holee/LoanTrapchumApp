using Domains.Models;

namespace Applications.Requests.Lists.Entities;

public class DepartmentDto :BaseEntity
{
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; } = string.Empty;
    public string Description { get; set; }=string.Empty;
    public bool IsActive { get; set; } = true;

}
