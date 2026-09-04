namespace Application.Features.Settings.Dto;

public class AppSettingDtoCreate
{
    public string? Code { get; set; }
    public string? Value { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; } = true;

} 
