namespace Applications.Requests.Identity
{
    public sealed class UserRoleDtoUpdate
    {
        public long UserId { get; set; }
        public string RoleName { get; set; }

        public UserRoleDtoUpdate(long userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

        public UserRoleDtoUpdate()
        {
            UserId = 0;
            RoleName= string.Empty; 
        }
    }

}
