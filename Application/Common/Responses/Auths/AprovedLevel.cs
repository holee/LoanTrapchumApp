using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Auths
{
    public record AprovedLevel(long UserId, string? UserName = null,string? Description=null);

}
