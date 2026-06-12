using System.Collections.ObjectModel;

namespace Applications.Constants.Authorizations
{
    public class UserLevelPermissions
    {
        private static readonly AppPermission[] _all = new AppPermission[]
        {
            new(Resources.CreditOfficersDashboard,Actions.Read,Group.ManagementHierarchy, "Show Credit Officer Menu"),
            new(Resources.AdministratorsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Administrator Menu"),
            new(Resources.OperatedPerson,Actions.Read, Group.ManagementHierarchy, "Show Operated Person Menu"),
            new(Resources.LoansDashboard,Actions.Read, Group.ManagementHierarchy, "Show Manage Loan Menu"),
            new(Resources.SettingsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Setting Menu"),
            new(Resources.ReportsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Report Menu"),
            new(Resources.BranchesDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Branch State Menu"),
            new(Resources.AccountsDashboard,Actions.Read,Group.ManagementHierarchy, "Show Manage Account Menu"),
        };
        public static IReadOnlyList<AppUserPermissions> AllUserPermissions { get; } = new ReadOnlyCollection<AppUserPermissions>(_all.Select(x => new AppUserPermissions(x.Group, x.Action)).ToList());


    }


}
