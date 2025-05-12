namespace Domains.Models.Identity;

public sealed class Permissions
{
    public long Id { get; set; }  
    public string? Module { get; set; }
    public string? Action { get; set; }
    public string? Group { get; set; }
    public string? Description { get; set; }
    public bool? IsBasic { get; set; } = false;
}