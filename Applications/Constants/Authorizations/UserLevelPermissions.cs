using System.Collections.ObjectModel;

namespace Applications.Constants.Authorizations
{
    public class UserLevelPermissions
    {
        private static readonly AppPermission[] _all = new AppPermission[]
        {
            new(Modules.CreditOfficersDashboard,Feature.Read,Group.ManagementHierarchy, "Show Credit Officer Menu"),
            new(Modules.AdministratorsDashboard,Feature.Read,Group.ManagementHierarchy, "Show Administrator Menu"),
            new(Modules.OperatedPerson,Feature.Read, Group.ManagementHierarchy, "Show Operated Person Menu"),
            new(Modules.LoansDashboard,Feature.Read, Group.ManagementHierarchy, "Show Manage Loan Menu"),
            new(Modules.SettingsDashboard,Feature.Read,Group.ManagementHierarchy, "Show Manage Setting Menu"),
            new(Modules.ReportsDashboard,Feature.Read,Group.ManagementHierarchy, "Show Manage Report Menu"),
            new(Modules.BranchesDashboard,Feature.Read,Group.ManagementHierarchy, "Show Manage Branch State Menu"),
            new(Modules.AccountsDashboard,Feature.Read,Group.ManagementHierarchy, "Show Manage Account Menu"),
        };
        public static IReadOnlyList<AppUserPermissions> AllUserPermissions { get; } = new ReadOnlyCollection<AppUserPermissions>(_all.Select(x => new AppUserPermissions(x.Group, x.Action)).ToList());


    }


}
