namespace Domains.Models.Settings;

public sealed class SysSettings
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string Value { get; set; }=default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = false; 
}
