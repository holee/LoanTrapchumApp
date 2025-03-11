namespace Applications.Requests.Lists.Employees
{
    public sealed class EmployeeActivate(long employeeId, bool isActive, int editSequent)
    {
        public long EmployeeId { get;} = employeeId;
        public bool IsActive { get;} = isActive;
        public int EditSequent { get; } = editSequent;
    }
}
