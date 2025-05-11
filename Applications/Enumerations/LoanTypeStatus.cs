using System.ComponentModel;

namespace Applications.Enumerations
{
    public enum LoanTypeStatus
    {
        [Description("Normal")]
        Standard =1,
        [Description("Sub Standard")]
        SubStandard,
        [Description("Doubtful")]
        Doubtful,
        [Description("Loss")]
        Loss,
        [Description("Special Mention")]
        SpecialMention

    }
}
