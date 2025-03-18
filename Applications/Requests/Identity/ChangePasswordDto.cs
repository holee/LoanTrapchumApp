namespace Applications.Requests.Identity
{
    public sealed class ChangePasswordDto
    {
        public string UserId { get; set; }=string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

}
