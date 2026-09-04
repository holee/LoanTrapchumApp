using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICurrentUserServices
    {
        string? UserId { get; }
        string? UserName { get; }
        string? Email { get; }
        string? Role { get; }
        IReadOnlyList<string> Roles { get; }

        string? GetEmail();
        string? GetRole();
        string? GetUserId();
        string? GetUserName();
    }
}
