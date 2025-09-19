namespace Applications.Enumerations
{

    [Flags]
    public enum RequestStatus
    {
        All,
        New = 1,
        Requested,
        Checking,
        Approved,
        Denied,
        Rechecked,
        Completed,
        ReApproved,

    }
}
