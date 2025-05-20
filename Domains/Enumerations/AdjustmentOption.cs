using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Enumerations
{
    public enum AdjustmentOption
    {
        [Description("Pay Before payment schedule")]
        Before = 1,
        [Description("Pay After payment schedule")]
        After = 2,
        [Description("Skip payment schedule")]
        Skip = 3
    }
}
