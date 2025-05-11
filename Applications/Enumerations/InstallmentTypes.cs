using System.ComponentModel;

namespace Applications.Enumerations
{
    public enum CollectionFeeModes
    {  
        [Description("Flat")]
        Flat = 1,
        [Description("Declined")]
        Declined = 2,
    }

    public enum InstallmentTypes 
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
