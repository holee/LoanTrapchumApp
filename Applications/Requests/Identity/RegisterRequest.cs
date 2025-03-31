using Domains.Models.Files;

namespace Applications.Requests.Identity;

public sealed class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public int BranchRefId { get; set; } = 0;
    public Guid? EmployeeId { get; set; } = Guid.Empty;
    public FileUploadModel? FileUpload { get; set; }

}
