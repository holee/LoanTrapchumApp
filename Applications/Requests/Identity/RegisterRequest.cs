using Domains.Models.Files;

namespace Applications.Requests.Identity;

public sealed class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = "Pa$$w0rd";
    public string ConfirmPassword { get; set; } = "Pa$$w0rd";
    public string RoleName { get; set; } = string.Empty;
    public int BranchRefId { get; set; } = 0;
    public string? EmployeeId { get; set; } = string.Empty;
    public FileUploadModel? FileUpload { get; set; }

}
