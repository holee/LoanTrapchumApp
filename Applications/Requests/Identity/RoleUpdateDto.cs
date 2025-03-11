namespace Applications.Requests.Identity
{
    public class RoleUpdateDto
    {
        public string? Id { get; set; }
        public string? RoleName { get; set; } = string.Empty;
    }
}
