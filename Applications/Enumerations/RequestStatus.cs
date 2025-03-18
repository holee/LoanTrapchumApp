namespace Applications.Enumerations
{

    [Flags]
    public enum RequestStatus
    {
        All,
        New = 1,
        Requested,
        Verified,
        Aprove,
        Denied,
        Rechecked,
        Completed
    }
}
