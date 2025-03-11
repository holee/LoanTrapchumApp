namespace Domains.Models.Auth
{
    public enum PemissionSeting
    { 
        All = 1,
        CreateUser=2,
        DeleteUser=3,
        ChangePassword,
        SetPermission,
        ViewReport,
        CanApprove,
        CreateLoan,
        CanRequest
    }
}
