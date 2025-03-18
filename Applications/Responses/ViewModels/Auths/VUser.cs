using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Auths;

public class VUser
{
    public string Id { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? RoleName { get; set; }
    public long? EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [NotMapped]
    public string? FullName => $"{FirstName} {LastName}";
    public string? Gender { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool? IsActive { get; set; }
}
