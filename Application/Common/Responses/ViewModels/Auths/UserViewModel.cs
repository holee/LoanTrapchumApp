namespace Application.Responses.ViewModels.Auths
{
    public class UserViewModel
    {
        public string? UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<UserRolesViewModel> UserRoles { get; set; } = [];
    }
}
