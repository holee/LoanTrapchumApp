using System.ComponentModel;

namespace Application.Enumerations
{
    public enum CashStatusEnum  
    {
        [Description("Cash comes in through transaction")]
        IN = 1, 
        [Description("Cash goes through transaction")]
        OUT = 2,
        [Description("Branch Manager gets money from Cash Vault")]
        DAYOPEN = 3,
        [Description("Branch Manager settles money to Cash Vault")]
        DAYClOSE = 4,
        [Description("User will send money to other user")]
        SEND = 5,
        [Description("User will receive money from other user")]
        RECEIVE = 6,
        [Description("Manager will return the  money to Cash Vault")]
        VAULTIN = 7,
        [Description("Manager will get the money from Cash Vault")]
        VAULTOUT = 8

    }
}
