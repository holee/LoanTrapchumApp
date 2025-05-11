using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Enumerations
{


    public enum RepaymentModes
    {
        [Description("Balloon")]
        Balloon =1,
        [Description("Decline")]
        Declined = 2,
        [Description("Flat")]
        IRR = 3,
        [Description("Annuity")]
        Annurity =4
    }
}
