using Domains.Models.Files;

namespace Applications.Requests.Identity;

public sealed class RegisterRequest
{
    public required string Username { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required string ConfirmPassword { get; set; } = string.Empty;
    public string? RoleName { get; set; }
    public int BranchRefId { get; set; } = 0;
    public long EmployeeId { get; set; } = 0;
    public FileUploadModel? FileUpload { get; set; }

}
