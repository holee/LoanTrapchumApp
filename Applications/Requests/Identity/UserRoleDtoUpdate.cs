namespace Applications.Requests.Identity
{
    public sealed class UserRoleDtoUpdate
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public UserRoleDtoUpdate(string userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

        public UserRoleDtoUpdate()
        {
            UserId = string.Empty;
            RoleName = string.Empty;
        }
    }

}
