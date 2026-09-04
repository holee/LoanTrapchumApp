using System.ComponentModel;

namespace Application.Commons.Authentications;

public enum RoleEnum
{
    [Description("Super Administrator")]
    SuperAdmin = 1,
    [Description("Administrator")]
    Admin,
    [Description("Chief Executive Officer")]
    Ceo,
    [Description("Regional Branch Manager")]
    Rbm,
    [Description("District Branch Manager")]
    Dbm,
    [Description("Manager")]
    Manager,
    [Description("Branch Manager")]
    Bm,
    [Description("Chief Branch Manager")]
    ChiefBm,
    [Description("Senior Accountant")]
    SeniorAccount,
    [Description("Chief Accountant")]
    ChiefAccount,
    [Description("Accountant")]
    Account,
    [Description("Teller")]
    Teller,
    [Description("Credit Officer")]
    Co
}
