using System.Collections.ObjectModel;

namespace Applications.Constants.Authorizations;

public sealed class ApplicationPermissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
        new(Modules.Branches,Actions.Read, Group.Administrator, "Read Branch",true),
        new(Modules.Branches,Actions.Create, Group.Administrator, "Create Branch"),
        new(Modules.Branches,Actions.Update, Group.Administrator, "Update Branch"),
        new(Modules.Branches,Actions.Delete, Group.Administrator, "Delete Branch"),
        new(Modules.Departments,Actions.Read, Group.Administrator, "Read Department",true),
        new(Modules.Departments,Actions.Create, Group.Administrator, "Create Department"),
        new(Modules.Departments,Actions.Update, Group.Administrator, "Update Department"),
        new(Modules.Departments,Actions.Delete, Group.Administrator, "Delete Department"),
        new(Modules.Collaterals,Actions.Read, Group.Operation, "Read Collateral",true),
        new(Modules.Collaterals,Actions.Create,Group.Operation, "Create Collateral",true),
        new(Modules.Collaterals,Actions.Update,Group.Operation, "Update Collateral"),
        new(Modules.Collaterals,Actions.Delete,Group.Operation, "Delete Collateral"),
        
        
        new(Modules.Currencies,Actions.Read, Group.Operation, "Read Currency",true),
        new(Modules.Currencies,Actions.Create, Group.Administrator, "Create Currency"),
        new(Modules.Currencies,Actions.Update, Group.Administrator, "Update Currency"),
        new(Modules.Currencies,Actions.Delete, Group.Administrator, "Delete Currency"),
        
        new(Modules.Employees,Actions.Read, Group.Operation, "Read Employees",true),
        new(Modules.Employees,Actions.Create, Group.Administrator, "Create Employees",true),
        new(Modules.Employees,Actions.Update, Group.Administrator, "Update Employees",true),
        new(Modules.Employees,Actions.Delete, Group.Administrator, "Delete Employees"),

        new(Modules.Vendors,Actions.Read, Group.Operation, "Read Vendor",true),
        new(Modules.Vendors,Actions.Update, Group.Operation, "Update Vendor",true),
        new(Modules.Vendors,Actions.Create, Group.Operation, "Create Vendor",true),
        new(Modules.Vendors,Actions.Delete, Group.Operation, "Delete Vendor"),

        new(Modules.Customers,Actions.Read, Group.Operation, "Read Customer",true),
        new(Modules.Customers,Actions.Create, Group.Operation, "Create Customer",true),
        new(Modules.Customers,Actions.Update, Group.Operation, "Update Customer",true),
        new(Modules.Customers,Actions.Delete, Group.Operation, "Delete Customer"),

        new(Modules.OtherNames,Actions.Read, Group.Operation, "Read OtherName", true),
        new(Modules.OtherNames,Actions.Create, Group.Operation, "Create OtherName", true),
        new(Modules.OtherNames,Actions.Update, Group.Operation, "Update OtherName", true),
        new(Modules.OtherNames,Actions.Delete,Group.Administrator, "Delete OtherName"),

        new(Modules.Roles,Actions.Read, Group.Operation, "Read Roles"),
        new(Modules.Roles,Actions.Create, Group.Administrator, "Create Roles"),
        new(Modules.Roles,Actions.Update, Group.Administrator, "Update Roles"),
        new(Modules.Roles,Actions.Delete, Group.Administrator, "Delete Roles"),
        new(Modules.Roles,Actions.Grant, Group.Administrator, "Assign Permissions to role"),
        new(Modules.Users,Actions.Read, Group.Administrator, "Read Users"),
        new(Modules.Users,Actions.Create, Group.Administrator, "Create Users"),
        new(Modules.Users,Actions.Update, Group.Administrator, "Update Users"),
        new(Modules.Users,Actions.Delete, Group.Administrator, "Delete Users"),
        new(Modules.Users,Actions.Grant, Group.Administrator, "Assign Permissions to role"),
        
        new(Modules.Journals,Actions.Read, Group.Accounting, "Read Journal", true),
        new(Modules.Journals,Actions.Create, Group.Accounting, "Create Journal", true),
        new(Modules.Journals,Actions.Update, Group.Accounting, "Update Journal", true),
        new(Modules.Journals,Actions.Delete, Group.Accounting, "Delete Journal", true),
        
        new(Modules.Ledgers,Actions.Read, Group.Accounting, "Read Ledger", true),
        new(Modules.Ledgers,Actions.Create, Group.Accounting, "Create Ledger", true),
        new(Modules.Ledgers,Actions.Update, Group.Accounting, "Update Ledger", true),
        new(Modules.Ledgers,Actions.Delete, Group.Accounting, "Delete Ledger", true),
        
        new(Modules.Loans,Actions.Read, Group.Operation, "Read Loan", true),
        new(Modules.Loans,Actions.Create, Group.Operation, "Create Loan", true),
        new(Modules.Loans,Actions.Update, Group.Operation, "Update Loan", true),
        new(Modules.Loans,Actions.Delete, Group.Operation, "Delete Loan", true),
        new(Modules.Loans,Actions.Reschedule, Group.Operation, "Reschedule Loan", true),
        
        new(Modules.LoanRequest,Actions.Read, Group.Operation, "Read Loan Request", true),
        new(Modules.LoanRequest,Actions.Create, Group.Operation, "Create Loan Request", true),
        new(Modules.LoanRequest,Actions.Update, Group.Operation, "Update Loan Request", true),
        new(Modules.LoanRequest,Actions.Delete, Group.Operation, "Delete Loan Request", true),
        new(Modules.LoanRequest,Actions.Disburse, Group.Operation, "Loan Disburse", true),
        new(Modules.LoanRequest,Actions.RejectRequest, Group.Operation, "Reject Loan Request", true),
        new(Modules.LoanRequest,Actions.ApproveRequest, Group.Operation, "Approve Request Loan", true),
       
        new(Modules.Repayments,Actions.Read, Group.Operation, "Read Payment", true),
        new(Modules.Repayments,Actions.Create, Group.Operation, "Create Payment", true),
        new(Modules.Repayments,Actions.Update, Group.Operation, "Update Payment", true),
        new(Modules.Repayments,Actions.Delete, Group.Operation, "Delete Payment", true),
        new(Modules.Repayments,Actions.MultiplePayment, Group.Accounting, "Allow Multiple Payment", true),
        new(Modules.Repayments,Actions.PayOff, Group.Accounting, "Allow Pay Off Loan", true),
        new(Modules.Repayments,Actions.WriteOff, Group.Accounting, "Allow Write Off Loan", true),
        new(Modules.Repayments,Actions.RecievePayment, Group.Accounting, "Allow Recieve Payment Loan", true),
        
        new(Modules.Transactions,Actions.Read, Group.Accounting, "Read Transaction", true),
        new(Modules.Transactions,Actions.Create, Group.Accounting, "Create Transaction", true),
        new(Modules.Transactions,Actions.Update, Group.Accounting, "Update Transaction", true),
        new(Modules.Transactions,Actions.Delete, Group.Accounting, "Delete Transaction", true),
        new(Modules.Transactions,Actions.VaultIn, Group.Accounting, "Start VaultIn", true),
        new(Modules.Transactions,Actions.VaultOut,Group.Accounting, "Start ValaultOut", true),
        new(Modules.Transactions,Actions.OpenBranch, Group.Accounting, "Open branch", true),
        new(Modules.Transactions,Actions.CloseBranch, Group.Accounting, "Close Branch", true),
        new(Modules.Transactions,Actions.ExchangeMoney, Group.Accounting, "Money exchange", true),

        new(Modules.Reports,Actions.Delete, Group.Operation, "Delete Report", true),
        new(Modules.Reports,Actions.Read, Group.Operation, "Read Report", true),
        new(Modules.Reports,Actions.Create, Group.Operation, "Create Report", true),
        new(Modules.Reports,Actions.Update, Group.Operation, "Update Report", true),
        
        new(Modules.AccountsDashboard,Actions.Read, Group.Operation, "Show Manage Account Menu",true),
        new(Modules.AdministratorsDashboard,Actions.Read, Group.Operation, "Show Administrator Menu"),
        new(Modules.BranchesDashboard,Actions.Read, Group.Operation, "Show Manage Branch State Menu",true),
        new(Modules.CreditOfficersDashboard,Actions.Read, Group.Operation, "Show Credit Officer Menu",true),
        new(Modules.CurrenciesDashboard,Actions.Read, Group.Operation, "Show Currency Menu"),
        new(Modules.OperatedPerson,Actions.Read, Group.Operation, "Show Operated Person Menu"),
        new(Modules.LoansDashboard,Actions.Read, Group.Operation, "Show Manage Loan Menu",true),
        new(Modules.Reports,Actions.Read, Group.Operation, "Read Report",true),
        new(Modules.SettingsDashboard,Actions.Read, Group.Operation, "Show Setting"),
        new(Modules.DepartmentsDashboard,Actions.Read, Group.Operation, "Show Department"),

        new(Modules.Dashboard,Actions.Read,Group.AccountDashboard, "Account Dashboard"),
        new(Modules.Dashboard,Actions.Read,Group.AdminDashboard, "Administrator Dashboard"),
        new(Modules.Dashboard,Actions.Read,Group.CoDashboard, "Credit Officer Dashboard"),
        new(Modules.Dashboard,Actions.Read,Group.CeoDashboard, "CEO Dashboard"),
        new(Modules.Dashboard,Actions.Read,Group.BbDashboard, "Regional Branch Manager"),
        new(Modules.Dashboard,Actions.Read,Group.BbDashboard,   "Branch Manager"),
        new(Modules.Dashboard,Actions.Read,Group.ReportDashboard, "Report Dashboard"),
        new(Modules.Dashboard,Actions.Read,Group.CurrencyDashboard,   "Currency Dashboard"),
        new(Modules.Dashboard,Actions.Read,Group.CurrencyDashboard,   "Loan Request Dashboard"),
        new(Modules.Dashboard,Actions.Read,Group.CurrencyDashboard,   "Loan Dashboard")

    };

    public static IReadOnlyList<AppPermission> AllPermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.ToArray());
    public static IReadOnlyList<AppPermission> AdminRolePermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic == false).ToArray());
    public static IReadOnlyList<AppPermission> DashboardPermission { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Module == Modules.AdministratorsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> AdminDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Module == Modules.AdministratorsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> CoDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Group == Modules.CreditOfficersDashboard).ToArray());
    public static IReadOnlyList<AppPermission> AccountDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Module == Modules.AccountsDashboard && p.Module == Modules.AccountsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> UserPermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic == true).ToArray());


}
