using Domains.Models.Lists.Employees;

namespace Domains.Models.Lists
{
    public class Department:BaseEntity
    {
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Employee>? Employees { get; set; }

    }
}
