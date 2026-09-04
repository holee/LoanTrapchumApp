namespace Application.Features.Identity.Roles.Requests
{
    public class RoleUpdateDto
    {
        public string? Id { get; set; }
        public string? RoleName { get; set; } = string.Empty;
    }
}
