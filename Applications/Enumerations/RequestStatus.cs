namespace Applications.Enumerations;


[Flags]
public enum RequestStatus
{
    All,
    New = 1,
    Requested = 2,
    Checking = 3,
    Approved = 4,
    Denied = 5,
    Rechecked = 6,
    Completed = 7,
    ReApproved = 8,
    InProgress​ = 9,
    Rejected = 10,
    Save = 11,
    Sending = 12,
    Rechecking = 13,
}
