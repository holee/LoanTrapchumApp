using System.Collections.ObjectModel;

namespace Applications.Constants.Authorizations;

public sealed class ApplicationPermissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
        new(Resources.Branches,Actions.Read, Group.Administrator, "Read Branch",true),
        new(Resources.Branches,Actions.Create, Group.Administrator, "Create Branch"),
        new(Resources.Branches,Actions.Update, Group.Administrator, "Update Branch"),
        new(Resources.Branches,Actions.Delete, Group.Administrator, "Delete Branch"),
        new(Resources.Departments,Actions.Read, Group.Administrator, "Read Department",true),
        new(Resources.Departments,Actions.Create, Group.Administrator, "Create Department"),
        new(Resources.Departments,Actions.Update, Group.Administrator, "Update Department"),
        new(Resources.Departments,Actions.Delete, Group.Administrator, "Delete Department"),
        new(Resources.Collaterals,Actions.Read, Group.Operation, "Read Collateral",true),
        new(Resources.Collaterals,Actions.Create,Group.Operation, "Create Collateral",true),
        new(Resources.Collaterals,Actions.Update,Group.Operation, "Update Collateral"),
        new(Resources.Collaterals,Actions.Delete,Group.Operation, "Delete Collateral"),
        
        
        new(Resources.Currencies,Actions.Read, Group.Operation, "Read Currency",true),
        new(Resources.Currencies,Actions.Create, Group.Administrator, "Create Currency"),
        new(Resources.Currencies,Actions.Update, Group.Administrator, "Update Currency"),
        new(Resources.Currencies,Actions.Delete, Group.Administrator, "Delete Currency"),
        
        new(Resources.Employees,Actions.Read, Group.Operation, "Read Employees",true),
        new(Resources.Employees,Actions.Create, Group.Administrator, "Create Employees",true),
        new(Resources.Employees,Actions.Update, Group.Administrator, "Update Employees",true),
        new(Resources.Employees,Actions.Delete, Group.Administrator, "Delete Employees"),

        new(Resources.Vendors,Actions.Read, Group.Operation, "Read Vendor",true),
        new(Resources.Vendors,Actions.Update, Group.Operation, "Update Vendor",true),
        new(Resources.Vendors,Actions.Create, Group.Operation, "Create Vendor",true),
        new(Resources.Vendors,Actions.Delete, Group.Operation, "Delete Vendor"),

        new(Resources.Customers,Actions.Read, Group.Operation, "Read Customer",true),
        new(Resources.Customers,Actions.Create, Group.Operation, "Create Customer",true),
        new(Resources.Customers,Actions.Update, Group.Operation, "Update Customer",true),
        new(Resources.Customers,Actions.Delete, Group.Operation, "Delete Customer"),

        new(Resources.OtherNames,Actions.Read, Group.Operation, "Read OtherName", true),
        new(Resources.OtherNames,Actions.Create, Group.Operation, "Create OtherName", true),
        new(Resources.OtherNames,Actions.Update, Group.Operation, "Update OtherName", true),
        new(Resources.OtherNames,Actions.Delete,Group.Administrator, "Delete OtherName"),

        new(Resources.Roles,Actions.Read, Group.Operation, "Read Roles"),
        new(Resources.Roles,Actions.Create, Group.Administrator, "Create Roles"),
        new(Resources.Roles,Actions.Update, Group.Administrator, "Update Roles"),
        new(Resources.Roles,Actions.Delete, Group.Administrator, "Delete Roles"),
        new(Resources.Roles,Actions.Grant, Group.Administrator, "Assign Permissions to role"),
        new(Resources.Users,Actions.Read, Group.Administrator, "Read Users"),
        new(Resources.Users,Actions.Create, Group.Administrator, "Create Users"),
        new(Resources.Users,Actions.Update, Group.Administrator, "Update Users"),
        new(Resources.Users,Actions.Delete, Group.Administrator, "Delete Users"),
        new(Resources.Users,Actions.Grant, Group.Administrator, "Assign Permissions to role"),
        
        new(Resources.Journals,Actions.Read, Group.Accounting, "Read Journal", true),
        new(Resources.Journals,Actions.Create, Group.Accounting, "Create Journal", true),
        new(Resources.Journals,Actions.Update, Group.Accounting, "Update Journal", true),
        new(Resources.Journals,Actions.Delete, Group.Accounting, "Delete Journal", true),
        
        new(Resources.Ledgers,Actions.Read, Group.Accounting, "Read Ledger", true),
        new(Resources.Ledgers,Actions.Create, Group.Accounting, "Create Ledger", true),
        new(Resources.Ledgers,Actions.Update, Group.Accounting, "Update Ledger", true),
        new(Resources.Ledgers,Actions.Delete, Group.Accounting, "Delete Ledger", true),
        
        new(Resources.Loans,Actions.Read, Group.Operation, "Read Loan", true),
        new(Resources.Loans,Actions.Create, Group.Operation, "Create Loan", true),
        new(Resources.Loans,Actions.Update, Group.Operation, "Update Loan", true),
        new(Resources.Loans,Actions.Delete, Group.Operation, "Delete Loan", true),
        new(Resources.Loans,Actions.Reschedule, Group.Operation, "Reschedule Loan", true),
        
        new(Resources.LoanRequest,Actions.Read, Group.Operation, "Read Loan Request", true),
        new(Resources.LoanRequest,Actions.Create, Group.Operation, "Create Loan Request", true),
        new(Resources.LoanRequest,Actions.Update, Group.Operation, "Update Loan Request", true),
        new(Resources.LoanRequest,Actions.Delete, Group.Operation, "Delete Loan Request", true),
        new(Resources.LoanRequest,Actions.Disburse, Group.Operation, "Loan Disburse", true),
        new(Resources.LoanRequest,Actions.RejectRequest, Group.Operation, "Reject Loan Request", true),
        new(Resources.LoanRequest,Actions.ApproveRequest, Group.Operation, "Approve Request Loan", true),
       
        new(Resources.Repayments,Actions.Read, Group.Operation, "Read Payment", true),
        new(Resources.Repayments,Actions.Create, Group.Operation, "Create Payment", true),
        new(Resources.Repayments,Actions.Update, Group.Operation, "Update Payment", true),
        new(Resources.Repayments,Actions.Delete, Group.Operation, "Delete Payment", true),
        new(Resources.Repayments,Actions.MultiplePayment, Group.Accounting, "Allow Multiple Payment", true),
        new(Resources.Repayments,Actions.PayOff, Group.Accounting, "Allow Pay Off Loan", true),
        new(Resources.Repayments,Actions.WriteOff, Group.Accounting, "Allow Write Off Loan", true),
        new(Resources.Repayments,Actions.ReceivePayment, Group.Accounting, "Allow Recieve Payment Loan", true),
        
        new(Resources.Transactions,Actions.Read, Group.Accounting, "Read Transaction", true),
        new(Resources.Transactions,Actions.Create, Group.Accounting, "Create Transaction", true),
        new(Resources.Transactions,Actions.Update, Group.Accounting, "Update Transaction", true),
        new(Resources.Transactions,Actions.Delete, Group.Accounting, "Delete Transaction", true),
        new(Resources.Transactions,Actions.VaultIn, Group.Accounting, "Start VaultIn", true),
        new(Resources.Transactions,Actions.VaultOut,Group.Accounting, "Start ValaultOut", true),
        new(Resources.Transactions,Actions.OpenBranch, Group.Accounting, "Open branch", true),
        new(Resources.Transactions,Actions.CloseBranch, Group.Accounting, "Close Branch", true),
        new(Resources.Transactions,Actions.ExchangeMoney, Group.Accounting, "Money exchange", true),

        new(Resources.Reports,Actions.Delete, Group.Operation, "Delete Report", true),
        new(Resources.Reports,Actions.Read, Group.Operation, "Read Report", true),
        new(Resources.Reports,Actions.Create, Group.Operation, "Create Report", true),
        new(Resources.Reports,Actions.Update, Group.Operation, "Update Report", true),
        
        new(Resources.AccountsDashboard,Actions.Read, Group.Operation, "Show Manage Account Menu",true),
        new(Resources.AdministratorsDashboard,Actions.Read, Group.Operation, "Show Administrator Menu"),
        new(Resources.BranchesDashboard,Actions.Read, Group.Operation, "Show Manage Branch State Menu",true),
        new(Resources.CreditOfficersDashboard,Actions.Read, Group.Operation, "Show Credit Officer Menu",true),
        new(Resources.CurrenciesDashboard,Actions.Read, Group.Operation, "Show Currency Menu"),
        new(Resources.OperatedPerson,Actions.Read, Group.Operation, "Show Operated Person Menu"),
        new(Resources.LoansDashboard,Actions.Read, Group.Operation, "Show Manage Loan Menu",true),
        new(Resources.Reports,Actions.Read, Group.Operation, "Read Report",true),
        new(Resources.SettingsDashboard,Actions.Read, Group.Operation, "Show Setting"),
        new(Resources.DepartmentsDashboard,Actions.Read, Group.Operation, "Show Department"),

        new(Resources.Dashboard,Actions.Read,Group.AccountDashboard, "Account Dashboard"),
        new(Resources.Dashboard,Actions.Read,Group.AdminDashboard, "Administrator Dashboard"),
        new(Resources.Dashboard,Actions.Read,Group.CoDashboard, "Credit Officer Dashboard"),
        new(Resources.Dashboard,Actions.Read,Group.CeoDashboard, "CEO Dashboard"),
        new(Resources.Dashboard,Actions.Read,Group.BbDashboard, "Regional Branch Manager"),
        new(Resources.Dashboard,Actions.Read,Group.BbDashboard,   "Branch Manager"),
        new(Resources.Dashboard,Actions.Read,Group.ReportDashboard, "Report Dashboard"),
        new(Resources.Dashboard,Actions.Read,Group.CurrencyDashboard,   "Currency Dashboard"),
        new(Resources.Dashboard,Actions.Read,Group.CurrencyDashboard,   "Loan Request Dashboard"),
        new(Resources.Dashboard,Actions.Read,Group.CurrencyDashboard,   "Loan Dashboard")

    };

    public static IReadOnlyList<AppPermission> AllPermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.ToArray());
    public static IReadOnlyList<AppPermission> AdminRolePermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic == false).ToArray());
    public static IReadOnlyList<AppPermission> DashboardPermission { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Resource == Resources.AdministratorsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> AdminDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Resource == Resources.AdministratorsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> CoDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Group == Resources.CreditOfficersDashboard).ToArray());
    public static IReadOnlyList<AppPermission> AccountDashboard { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.Resource == Resources.AccountsDashboard && p.Resource == Resources.AccountsDashboard).ToArray());
    public static IReadOnlyList<AppPermission> UserPermissions { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic == true).ToArray());


}
