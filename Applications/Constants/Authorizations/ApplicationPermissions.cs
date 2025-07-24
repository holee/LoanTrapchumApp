using System.Collections.ObjectModel;

namespace Applications.Constants.Authorizations;

public sealed class ApplicationPermissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
        new(Modules.Branches,Feature.Read, Group.Administrator, "Read Branch",true),
        new(Modules.Branches,Feature.Create, Group.Administrator, "Create Branch"),
        new(Modules.Branches,Feature.Update, Group.Administrator, "Update Branch"),
        new(Modules.Branches,Feature.Delete, Group.Administrator, "Delete Branch"),
        new(Modules.Departments,Feature.Read, Group.Administrator, "Read Department",true),
        new(Modules.Departments,Feature.Create, Group.Administrator, "Create Department"),
        new(Modules.Departments,Feature.Update, Group.Administrator, "Update Department"),
        new(Modules.Departments,Feature.Delete, Group.Administrator, "Delete Department"),
        new(Modules.Collaterals,Feature.Read, Group.Operation, "Read Collateral",true),
        new(Modules.Collaterals,Feature.Create,Group.Operation, "Create Collateral",true),
        new(Modules.Collaterals,Feature.Update,Group.Operation, "Update Collateral"),
        new(Modules.Collaterals,Feature.Delete,Group.Operation, "Delete Collateral"),
        new(Modules.Currencies,Feature.Read, Group.Operation, "Read Currency",true),
        new(Modules.Currencies,Feature.Create, Group.Administrator, "Create Currency"),
        new(Modules.Currencies,Feature.Update, Group.Administrator, "Update Currency"),
        new(Modules.Currencies,Feature.Delete, Group.Administrator, "Delete Currency"),
        new(Modules.Employees,Feature.Read, Group.Operation, "Read Employees",true),
        new(Modules.Employees,Feature.Create, Group.Administrator, "Create Employees",true),
        new(Modules.Employees,Feature.Update, Group.Administrator, "Update Employees",true),
        new(Modules.Employees,Feature.Delete, Group.Administrator, "Delete Employees"),
        new(Modules.Vendors,Feature.Read, Group.Operation, "Read Vendor",true),
        new(Modules.Vendors,Feature.Update, Group.Operation, "Update Vendor",true),
        new(Modules.Vendors,Feature.Create, Group.Operation, "Create Vendor",true),
        new(Modules.Vendors,Feature.Delete, Group.Operation, "Delete Vendor"),
        new(Modules.Customers,Feature.Read, Group.Operation, "Read Customer",true),
        new(Modules.Customers,Feature.Create, Group.Operation, "Create Customer",true),
        new(Modules.Customers,Feature.Update, Group.Operation, "Update Customer",true),
        new(Modules.Customers,Feature.Delete, Group.Operation, "Delete Customer"),
        new(Modules.OtherNames,Feature.Read, Group.Operation, "Read OtherName", true),
        new(Modules.OtherNames,Feature.Create, Group.Operation, "Create OtherName", true),
        new(Modules.OtherNames,Feature.Update, Group.Operation, "Update OtherName", true),
        new(Modules.OtherNames,Feature.Delete,Group.Administrator, "Delete OtherName"),
        new(Modules.Roles,Feature.Read, Group.Operation, "Read Roles"),
        new(Modules.Roles,Feature.Create, Group.Administrator, "Create Roles"),
        new(Modules.Roles,Feature.Update, Group.Administrator, "Update Roles"),
        new(Modules.Roles,Feature.Delete, Group.Administrator, "Delete Roles"),
        new(Modules.Roles,RoleActionType.Grant, Group.Administrator, "Assign Permissions to role"),
        new(Modules.Users,Feature.Read, Group.Administrator, "Read Users"),
        new(Modules.Users,Feature.Create, Group.Administrator, "Create Users"),
        new(Modules.Users,Feature.Update, Group.Administrator, "Update Users"),
        new(Modules.Users,Feature.Delete, Group.Administrator, "Delete Users"),
        new(Modules.Users,UserActionType.Grant, Group.Administrator, "Assign Permissions to role"),
        new(Modules.Journals,Feature.Read, Group.Accounting, "Read Journal", true),
        new(Modules.Journals,Feature.Create, Group.Accounting, "Create Journal", true),
        new(Modules.Journals,Feature.Update, Group.Accounting, "Update Journal", true),
        new(Modules.Journals,Feature.Delete, Group.Accounting, "Delete Journal", true),
        new(Modules.Ledgers,Feature.Read, Group.Accounting, "Read Ledger", true),
        new(Modules.Ledgers,Feature.Create, Group.Accounting, "Create Ledger", true),
        new(Modules.Ledgers,Feature.Update, Group.Accounting, "Update Ledger", true),
        new(Modules.Ledgers,Feature.Delete, Group.Accounting, "Delete Ledger", true),
        new(Modules.Loans,Feature.Read, Group.Operation, "Read Loan", true),
        new(Modules.Loans,Feature.Create, Group.Operation, "Create Loan", true),
        new(Modules.Loans,Feature.Update, Group.Operation, "Update Loan", true),
        new(Modules.Loans,Feature.Delete, Group.Operation, "Delete Loan", true),
        new(Modules.Loans,Feature.Reschedule, Group.Operation, "Reschedule Loan", true),
        new(Modules.LoanRequest,Feature.Read, Group.Operation, "Read Loan Request", true),
        new(Modules.LoanRequest,Feature.Create, Group.Operation, "Create Loan Request", true),
        new(Modules.LoanRequest,Feature.Update, Group.Operation, "Update Loan Request", true),
        new(Modules.LoanRequest,Feature.Delete, Group.Operation, "Delete Loan Request", true),
        new(Modules.LoanRequest,Feature.Disburse, Group.Operation, "Loan Disburse", true),
        new(Modules.LoanRequest,Feature.RejectRequest, Group.Operation, "Reject Loan Request", true),
        new(Modules.LoanRequest,Feature.ApproveRequest, Group.Operation, "Approve Request Loan", true),
        new(Modules.Repayments,Feature.Read, Group.Operation, "Read Payment", true),
        new(Modules.Repayments,Feature.Create, Group.Operation, "Create Payment", true),
        new(Modules.Repayments,Feature.Update, Group.Operation, "Update Payment", true),
        new(Modules.Repayments,Feature.Delete, Group.Operation, "Delete Payment", true),
        new(Modules.Repayments,Feature.MultiplePayment, Group.Accounting, "Allow Multiple Payment", true),
        new(Modules.Repayments,Feature.PayOff, Group.Accounting, "Allow Pay Off Loan", true),
        new(Modules.Transactions,Feature.Read, Group.Accounting, "Read Transaction", true),
        new(Modules.Transactions,Feature.Create, Group.Accounting, "Create Transaction", true),
        new(Modules.Transactions,Feature.Update, Group.Accounting, "Update Transaction", true),
        new(Modules.Transactions,Feature.Delete, Group.Accounting, "Delete Transaction", true),
        new(Modules.Transactions,Feature.VaultIn, Group.Accounting, "Start VaultIn", true),
        new(Modules.Transactions,Feature.VaultOut,Group.Accounting, "Start ValaultOut", true),
        new(Modules.Transactions,Feature.OpenBranch, Group.Accounting, "Open branch", true),
        new(Modules.Transactions,Feature.CloseBranch, Group.Accounting, "Close Branch", true),
        new(Modules.Transactions,Feature.ExchangeMoney, Group.Accounting, "Money exchange", true),
        new(Modules.Reports,Feature.Delete, Group.Operation, "Delete Report", true),
        new(Modules.Reports,Feature.Read, Group.Operation, "Read Report", true),
        new(Modules.Reports,Feature.Create, Group.Operation, "Create Report", true),
        new(Modules.Reports,Feature.Update, Group.Operation, "Update Report", true),
        
        new(Modules.AccountsDashboard,Feature.Read, Group.Operation, "Show Manage Account Menu",true),
        new(Modules.AdministratorsDashboard,Feature.Read, Group.Operation, "Show Administrator Menu"),
        new(Modules.BranchesDashboard,Feature.Read, Group.Operation, "Show Manage Branch State Menu",true),
        new(Modules.CreditOfficersDashboard,Feature.Read, Group.Operation, "Show Credit Officer Menu",true),
        new(Modules.CurrenciesDashboard,Feature.Read, Group.Operation, "Show Currency Menu"),
        new(Modules.OperatedPerson,Feature.Read, Group.Operation, "Show Operated Person Menu"),
        new(Modules.LoansDashboard,Feature.Read, Group.Operation, "Show Manage Loan Menu",true),
        new(Modules.Reports,Feature.Read, Group.Operation, "Read Report",true),
        new(Modules.SettingsDashboard,Feature.Read, Group.Operation, "Show Setting"),
        new(Modules.DepartmentsDashboard,Feature.Read, Group.Operation, "Show Department"),

        new(Modules.Dashboard,Feature.Read,Group.AccountDashboard, "Account Dashboard"),
        new(Modules.Dashboard,Feature.Read,Group.AdminDashboard, "Administrator Dashboard"),
        new(Modules.Dashboard,Feature.Read,Group.CoDashboard, "Credit Officer Dashboard"),
        new(Modules.Dashboard,Feature.Read,Group.CeoDashboard, "CEO Dashboard"),
        new(Modules.Dashboard,Feature.Read,Group.BbDashboard, "Regional Branch Manager"),
        new(Modules.Dashboard,Feature.Read,Group.BbDashboard,   "Branch Manager"),
        new(Modules.Dashboard,Feature.Read,Group.ReportDashboard, "Report Dashboard"),
        new(Modules.Dashboard,Feature.Read,Group.CurrencyDashboard,   "Currency Dashboard"),
        new(Modules.Dashboard,Feature.Read,Group.CurrencyDashboard,   "Loan Request Dashboard"),
        new(Modules.Dashboard,Feature.Read,Group.CurrencyDashboard,   "Loan Dashboard")

    };

    public static IReadOnlyList<AppPermission> AllPermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.ToArray());
    public static IReadOnlyList<AppPermission> AdminRolePermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic == false).ToArray());
    public static IReadOnlyList<AppPermission> DashboardPermission { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Module == Modules.AdministratorsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> AdminDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Module == Modules.AdministratorsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> CoDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Group == Modules.CreditOfficersDashboard).ToArray());
    public static IReadOnlyList<AppPermission> AccountDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Module == Modules.AccountsDashboard && p.Module == Modules.AccountsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> UserPermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic == true).ToArray());


}
