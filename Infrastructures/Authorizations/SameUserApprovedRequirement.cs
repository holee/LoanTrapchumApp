using Microsoft.AspNetCore.Authorization;

namespace Infrastructures.Authorizations;

public sealed class SameUserApprovedRequirement : IAuthorizationRequirement 
{

}