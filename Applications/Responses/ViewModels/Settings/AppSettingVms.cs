namespace Applications.Responses.ViewModels.Settings;

public sealed class AppSettingVms
{
    public int Id { get; set; }
    public string? Code { get; set; } = string.Empty;
    public string? Value { get; set; }=string.Empty;
    public string? Description { get; set; }=string.Empty;
    public bool IsActive { get; set; } = false; 
}
