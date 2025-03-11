using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Extensions.Identity;

public sealed class UserProfiles
{
    public long Id { get; set; }
    public string UserRefId { get; set; }=string.Empty;
    public string? FileTitle { get; set; } = string.Empty;
    public string? FileName { get; set; } = string.Empty;
    public string? FileUrl { get; set; }=string.Empty;
    public DateTime? Created { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public ApplicationUser? AppUser { get; set; }  
}
