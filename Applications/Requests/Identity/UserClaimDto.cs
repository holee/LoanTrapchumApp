namespace Applications.Requests.Identity
{
    public class UserClaimDto
    {
        public string? ClaimType { get; set; } = string.Empty;
        public bool? IsSelected { get; set; } = false;   
    }
}
