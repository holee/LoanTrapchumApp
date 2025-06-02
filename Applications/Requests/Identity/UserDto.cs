using Domains.Models.Identity;

namespace Applications.Requests.Identity;

public sealed class UserDto
{
    public UserDto()
    {
        UserName = string.Empty;
        Email = string.Empty;
        Role = string.Empty;
        IsActive = false;
        Roles = [];
    }
    public UserDto(string id, string userName, string email, string roles, bool isActive)
    {
        Id = id;
        UserName = userName;
        Email = email;
        Role = roles;
        IsActive = isActive;
    }

    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public List<string> Roles { get; set; } = [];
    public UserProfiles? Profile { get; set; }
}
