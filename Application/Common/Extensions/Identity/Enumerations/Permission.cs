using System.ComponentModel.DataAnnotations;

namespace Application.Extensions.Identity.Enumerations;

public enum Permission
{
    [Display(Description = "Create User")]
    CreateUser = 1,
    [Display(Description = "View User")]
    ViewUser,
    [Display(Description = "Edit User")]
    EditUser,
    [Display(Description = "Delete User")]
    DeleteUser,
    [Display(Description = "Disable User")]
    DisableUser,
    [Display(Description = "Create Role")]
    CreateRole,
    [Display(Description = "View Role")]
    ViewRole,
    [Display(Description = "Edit Role")]
    EditRole,
    [Display(Description = "Delete Role")]
    DeleteRole,
    [Display(Description = "Assign Role")]
    AssignRole,
    [Display(Description = "Assign Persmission")]
    AssignPersmission,
    [Display(Description = "Create Branch")]
    CreateBranch,
    [Display(Description = "Edit Branch")]
    EditBranch,
    [Display(Description = "Delete Branch")]
    DeleteBranch,
    [Display(Description = "View Branch")]
    ViewBranch,
    [Display(Description = "Open Branch")]
    OpenBranch,
    [Display(Description = "Close Branch")]
    CloseBranch,
    [Display(Description = "Create Ledger")]
    CreateLedger,
    [Display(Description = "View Ledger")]
    ViewLedger,
    [Display(Description = "Delete Ledger")]
    DeleteLedger,
    [Display(Description = "Edit Ledger")]
    EditLedger,
    [Display(Description = "Create User")]
    CreateJournal,
    [Display(Description = "View Journal")]
    ViewJournal,
    [Display(Description = "Edit Journal")]
    EditJournal,
    [Display(Description = "Delete Journal")]
    DeleteJournal,
    [Display(Description = "Create User")]
    ViewReport,
    [Display(Description = "Valault Out")]
    ValaultOut,
    [Display(Description = "Valault In")]
    ValaultIn,
    [Display(Description = "Create New Loan")]
    CreateNewLoan,
    [Display(Description = "Edit Loan")]
    EditLoan,
    [Display(Description = "ViewLoan")]
    ViewLoan,
    [Display(Description = "Delete Loan")]
    DeleteLoan,
}


public enum SystemPermissions 
{
    [Display(Description = "Create User,Role,Branch,Department,and others....")]
    Create = 1,
    [Display(Description = "View User,Role,Branch,Department,and others....")]
    View,
    [Display(Description = "Edit User,Role,Branch,Department,and others....")]
    Edit,
    [Display(Description = "Delete User,Role,Branch,Department,and others....")]
    Delete

}