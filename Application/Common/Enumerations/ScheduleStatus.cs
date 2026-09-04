using System.ComponentModel;

namespace Application.Enumerations
{
    public enum ScheduleStatus
    {
        All = 0,
        Paid = 1,
        Unpaid = 2,
        Cancel = 3,
        Partial = 4,
        OnDate = 5,
        Late = 6,
    }


    public enum LoanStatuses
    {
        [Description("Standard")]
        Standard = 1,
        [Description("Closed")]
        Close = 2,
        [Description("Deleted")]
        Deleted = 3,
        [Description("Active")]
        Active = 4,
        [Description("Lost")]
        Lost,
        [Description("RollOver")]
        RollOver,
        [Description("Sub Standard")]
        SubStandard,
        [Description("Doubtful")]
        Doubtful,
        [Description("Special Mention")]
        SpecialMention
    }

}

