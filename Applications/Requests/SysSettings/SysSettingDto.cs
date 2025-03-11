namespace Applications.Requests.SysSettings;

public class SysSettingDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? AppliedValue { get; set; } 
    public string? Description { get; set; }
    public bool? Status { get; set; } = true;
}
