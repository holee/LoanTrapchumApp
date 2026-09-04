namespace Application.Responses.ViewModels.Auths
{
    public class UserRolesViewModel
    {
        public UserRolesViewModel()
        {
            RoleId = string.Empty;
            RoleName = string.Empty;
            IsSelected = false;
        }
        public UserRolesViewModel(string roleId, string userId, string? roleName, bool isSelected)
        {
            RoleId = roleId;
            RoleName = roleName;
            IsSelected = isSelected;
        }

        public string RoleId { get; set; }
        public string? RoleName { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
    }


}
