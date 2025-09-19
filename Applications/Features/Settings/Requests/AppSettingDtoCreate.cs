namespace Applications.Features.Settings.Requests;

public class AppSettingDtoCreate
{
    public string? Code { get; set; }
    public string? AppliedValue { get; set; }
    public string? Description { get; set; }
    public bool? Status { get; set; } = true;
} 
