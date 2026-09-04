namespace Application.Features.Identity.Users
{
    public sealed class UserProfilesCreateDto
    {
        public long UserRefId { get; }
        public string? FileTitle { get; }
        public string? FileName { get; }

        public UserProfilesCreateDto(long userRefId, string? fileTitle, string? fileName)
        {
            UserRefId = userRefId;
            FileTitle = fileTitle;
            FileName = fileName;
        }
    }
}
