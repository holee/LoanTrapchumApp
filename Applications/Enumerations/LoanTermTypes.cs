using System.ComponentModel;

namespace Applications.Enumerations
{
    public enum LoanTermTypes
    {
        [Description("Normal")]
        Normal =1,
        [Description("Flat Rate")]
        FlatRate,
        [Description("Armortization")]
        Armortization
    }
}
