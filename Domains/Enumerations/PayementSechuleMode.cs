using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Enumerations
{
    public enum PayementSechuleMode
    {
        [Description("Pay Before payment schedule")]
        Before,
        [Description("Pay After payment schedule")]
        After,
        [Description("Skip payment schedule")]
        Skip    
    }
}
