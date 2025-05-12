using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Responses.ViewModels.Auths
{
    public sealed class PermissionsVm
    {
        public long Id { get; set; }
        public string? Module { get; set; }
        public string? Action { get; set; }
        public string? Group { get; set; }
        public string? Description { get; set; }
        public bool? IsBasic { get; set; } = false;
    }
}
