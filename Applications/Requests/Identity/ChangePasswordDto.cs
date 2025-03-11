namespace Applications.Requests.Identity
{
    public sealed class ChangePasswordDto
    {
        public long UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

}
