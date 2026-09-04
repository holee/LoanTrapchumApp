using System.ComponentModel;

namespace Application.Enumerations
{
    public enum CollateralTypes
    {
        [Description("Property, Land, House, etc")]
        Property = 1,
        [Description("Fixed Deposit")]
        FD = 2,
        [Description("Personal Guarantee")]
        Personal = 3,
        [Description("Cheque")]
        Cheque = 4,
        [Description("RemovableProperty, Job")]
        RemovableProperty = 5
    }
}
