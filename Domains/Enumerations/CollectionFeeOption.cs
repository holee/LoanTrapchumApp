using System.ComponentModel;

namespace Domains.Enumerations 
{
    public enum CollectionFeeOption
    {  
        [Description("Flat")]
        IRR = 1,
        [Description("Declined")]
        Declined = 2,
    }
}
