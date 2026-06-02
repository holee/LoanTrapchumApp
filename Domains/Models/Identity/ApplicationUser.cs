using Microsoft.AspNetCore.Identity;

namespace Domains.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string? NameRefId { get; set; }
    public int? BranchRefId { get; set; }
    public bool? IsActive { get; set; } = true;
    public int? EditSquence { get; set; } = 0;
    public string? RefreshToken { get; set; }
    public int? AprovedLevelId { get; set; }
    public DateTime? RefreshTokenExpiryDate { get; set; }
    public DateTime? CreatedDT { get; set; } = DateTime.Now;
    public DateTime? ModifiedDT { get; set; } = DateTime.Now;
    public UserProfiles? UserProfiles { get; set; }
}
