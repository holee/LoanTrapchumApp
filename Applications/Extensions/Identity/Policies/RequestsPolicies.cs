using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public partial class Policies  
{
    private const string req_prefix = "loanRequests";
    public const string CreateLoanRequest = $"{req_prefix}.{nameof(CreateLoanRequest)}";
    public const string UpdateLoanRequest = $"{req_prefix}.{nameof(UpdateLoanRequest)}";
    public const string DeleteLoanRequest = $"{req_prefix}.{nameof(DeleteLoanRequest)}";
    public const string ReadLoanRequest = $"{req_prefix}.{nameof(ReadLoanRequest)}";
    public const string ApproveLoanRequest = $"{req_prefix}.{nameof(ApproveLoanRequest)}";
    public const string Disburse = $"{req_prefix}.{nameof(Disburse)}";
    public const string RequestLoanRequest = $"{req_prefix}.{nameof(RequestLoanRequest)}";
    public const string RejectLoanRequest = $"{req_prefix}.{nameof(RejectLoanRequest)}";

    private static string read_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.Read);
    private static string update_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.Update);
    private static string create_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.Create);
    private static string delete_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.Delete);
    private static string approve_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.ApproveRequest);
    private static string reject_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.RejectRequest);
    private static string request_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.Request);
    private static string disburse_loanRequest = AppPermission.RuleFor(Modules.LoanRequest, Actions.Disburse);
     
    public static AuthorizationPolicy CanDisburseLoan()  
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.SeniorAccount, AppRoles.ChiefAccount)
                    .RequireClaim(AppClaim.Permission, disburse_loanRequest)
                    .Build();
    }


    public static AuthorizationPolicy CanCreateLoanRequest()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Co)
                    .RequireClaim(AppClaim.Permission, create_loanRequest)
                    .Build();
    }

    public static AuthorizationPolicy CanRequestLoanRequest() 
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Co)
                    .RequireClaim(AppClaim.Permission, request_loanRequest)
                    .Build();
    }
    public static AuthorizationPolicy CanUpdateLoanRequest()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireClaim(AppClaim.Permission, update_loanRequest)
                    .Build();
    }

    public static AuthorizationPolicy CanReadLoanRequest()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, read_loanRequest)
                    .Build();
    }

    public static AuthorizationPolicy CanDeleteLoanRequest() 
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, delete_loanRequest)
                    .Build();
    }

    public static AuthorizationPolicy CanAproveRequest()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, approve_loanRequest)
                    .Build();
    }

    public static AuthorizationPolicy CanRejectRequest()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireRole(AppRoles.Manager, AppRoles.Bm, AppRoles.Rbm, AppRoles.Dbm)
                    .RequireClaim(AppClaim.Permission, reject_loanRequest)
                    .Build();
    }




}
