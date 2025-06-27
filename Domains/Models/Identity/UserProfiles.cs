using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Identity;

public sealed class UserProfiles
{
    public string? UserRefId { get; set; }
    public string? FileName { get; set; } = string.Empty;
    public string? FileUrl { get; set; }=string.Empty;
    public DateTime? Created { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
}
