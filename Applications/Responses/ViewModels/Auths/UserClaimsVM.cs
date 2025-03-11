using Applications.Requests.Identity;

namespace Applications.Responses.ViewModels.Auths
{
    public class UserClaimsVm
    {
        public UserClaimsVm()
        {
            Claims = new List<UserClaimDto>();
        }
        public long? UserId { get; set; }
        public List<UserClaimDto>? Claims { get; set; }
    }

}
