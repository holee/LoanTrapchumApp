namespace Applications.Features.Settings.Dto;

public sealed class AppSettingDto
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
