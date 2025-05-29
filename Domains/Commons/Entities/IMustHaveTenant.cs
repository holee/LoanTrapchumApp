using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Commons.Entities;

public interface IMustHaveTenant
{
    string TenantId { get; set; }
}
