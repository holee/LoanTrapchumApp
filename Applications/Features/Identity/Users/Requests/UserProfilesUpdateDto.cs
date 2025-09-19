namespace Applications.Features.Identity.Users
{
    public sealed class UserProfilesUpdateDto   
    {
        public long Id { get; set; }
        public long UserRefId { get; set; }
        public string? FileTitle { get; set; }
        public string? FileName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
