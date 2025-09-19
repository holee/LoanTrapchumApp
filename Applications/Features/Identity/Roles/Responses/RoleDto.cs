namespace Applications.Features.Identity.Roles.Responses
{
    public class RoleDto
    {
        public string Id { get; set; }= string.Empty;
        public string? RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Description { get; set; } 
    }
}
