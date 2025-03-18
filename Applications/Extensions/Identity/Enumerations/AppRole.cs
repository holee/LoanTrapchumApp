using System.ComponentModel.DataAnnotations;

namespace Applications.Extensions.Identity.Enumerations;

public enum AppRole
{
    [Display(Description = "Super Administrator")]
    SuperAdmin,
    [Display(Description = "Administrator")]
    Admin,
    [Display(Description = "Chief Executive Officer")]
    Ceo,
    [Display(Description = "Reginal Branch Manager")]
    Rbm,
    [Display(Description = "Branch Manager")]
    Manager,
    [Display(Description = "Branch Manager")]
    Bm,
    [Display(Description = "Chief Branch Manager")]
    ChiefBm,
    [Display(Description = "Senior Account")]
    SeniorAccount,
    [Display(Description = "Chief Account")]
    ChiefAccount,
    [Display(Description = "Account")]
    Account,
    [Display(Description = "Teller")]
    Teller,
    [Display(Description = "Credit Officer")]
    Co

}