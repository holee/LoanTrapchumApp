using System.Text.Json.Serialization;

namespace Applications.Requests.Identity;

public class UsersResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; } = false;
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string Token { get; set; } = string.Empty;
    public int? BranchId { get; set; } 
    public string? BranchName { get; set; }=string.Empty;
    public int? DepartmentId { get; set; } 
    public string? DepartmentName { get;set; } = string.Empty;  
    [JsonIgnore] public string RefreshToken { get; set; }= string.Empty;
    [JsonIgnore] public DateTime? RefreshTokenExpiration { get; set; }    
}
