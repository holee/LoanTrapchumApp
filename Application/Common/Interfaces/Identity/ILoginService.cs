using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Identity.Users;

namespace Application.Interfaces.Identity
{
    public interface ILoginService
    {
        Task<Result<string>> LoginUser(LoginRequest request); 
    }
}
