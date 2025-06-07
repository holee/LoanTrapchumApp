namespace Applications.Enumerations
{

    [Flags]
    public enum RequestStatus
    {
        All,
        New = 1,
        Requested,
        Checking,
        Aprove,
        Denied,
        Rechecked,
        Completed,
        ReApproved,

    }
}
