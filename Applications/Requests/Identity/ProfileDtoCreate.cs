namespace Applications.Requests.Identity;

public sealed class ProfileDtoCreate
{
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; } = 0;
    public byte[]? FileContent { get; set; }
}
