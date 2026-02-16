using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Auths;

public sealed class VUser
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? RoleName { get; set; } 
    public bool CanApproved { get; set; } = false;
    public string? EmployeeRefId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    [NotMapped] public string? FullName => $"{FirstName} {LastName}";
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool? IsActive { get; set; }

}
