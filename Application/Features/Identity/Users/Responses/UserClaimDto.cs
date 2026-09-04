namespace Application.Features.Identity.Users
{
    public class UserClaimDto
    {
        public string? ClaimType { get; set; } = string.Empty;
        public bool? IsSelected { get; set; } = false;   
    }
}
