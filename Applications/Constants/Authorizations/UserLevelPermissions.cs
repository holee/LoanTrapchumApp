using System.Collections.ObjectModel;

namespace Applications.Constants.Authorizations
{
    public class UserLevelPermissions
    {
        private static readonly AppPermission[] _all = new AppPermission[]
        {
            new(Modules.CreditOfficersDashboard,Actions.Read,Group.ManagementHierarchy, "Show Credit Officer Menu"),
            new(Modules.AdministratorsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Administrator Menu"),
            new(Modules.OperatedPerson,Actions.Read, Group.ManagementHierarchy, "Show Operated Person Menu"),
            new(Modules.LoansDashboard,Actions.Read, Group.ManagementHierarchy, "Show Manage Loan Menu"),
            new(Modules.SettingsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Setting Menu"),
            new(Modules.ReportsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Report Menu"),
            new(Modules.BranchesDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Branch State Menu"),
            new(Modules.AccountsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Account Menu"),
        };
        public static IReadOnlyList<AppUserPermissions> AllUserPermissions { get; } = new ReadOnlyCollection<AppUserPermissions>(_all.Select(x => new AppUserPermissions(x.Group, x.Action)).ToList());


    }


}
