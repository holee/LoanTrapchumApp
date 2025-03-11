using Applications.Responses.ViewModels.Auths;

namespace Applications.Requests.Identity
{
    public class UserRolesClaimsDto
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<ClaimVm> Claims { get; set; } = new List<ClaimVm>();
        public List<string> Roles { get; set; } = new List<string>();
       
    }
}
