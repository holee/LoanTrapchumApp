using System.ComponentModel;

namespace Domains.Enumerations 
{

    public enum InstallmentOption 
    {
        [Description("every month")]
        Monthly = 1, 
        [Description("Pay every 2 weeks")]
        BiWeekly = 2,
        [Description("Pay every 2 weeks")] 
        Weekly = 3,
        [Description("Pay everyday")]
        Daily = 4
    }
}
