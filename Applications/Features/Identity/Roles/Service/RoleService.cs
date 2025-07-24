using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Identity.Roles.Service
{
    public sealed class RoleService
    {
        private readonly IRoleService _roleService;

        public RoleService(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task UpdateRoleAsync(RoleDtoUpdate role)
        {

        }

    }
}
