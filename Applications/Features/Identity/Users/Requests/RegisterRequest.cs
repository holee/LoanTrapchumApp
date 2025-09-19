using Domains.Models.Files;
using Domains.Shared;

namespace Applications.Features.Identity.Users;

public sealed class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = DefaultOption.DefaultPassword;
    public string ConfirmPassword { get; set; } = DefaultOption.DefaultPassword;
    public string RoleName { get; set; } = string.Empty;
    public int BranchRefId { get; set; } = 0;
    public string? EmployeeId { get; set; } = string.Empty;
    public FileModel? FileUpload { get; set; }

}
