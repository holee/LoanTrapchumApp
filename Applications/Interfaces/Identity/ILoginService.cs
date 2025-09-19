using Applications.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Features.Identity.Users;

namespace Applications.Interfaces.Identity
{
    public interface ILoginService
    {
        Task<Result<string>> LoginUser(LoginRequest request); 
    }
}
