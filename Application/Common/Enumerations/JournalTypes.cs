using System.ComponentModel;

namespace Application.Enumerations
{
    public enum JournalTypes 
    {
        [Description("Payment Voucher")]
        PV = 1,
        [Description("Fund Transfer")]
        FT = 2,
        [Description("Receipt Voucher")]
        RV = 3,
        [Description("Journal Voucher")]
        JV = 4
    }
}
