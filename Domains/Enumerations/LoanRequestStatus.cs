using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Enumerations
{
    public enum LoanRequestStatus
    {
        New = 1,
        Requested,
        Checking,
        Approved,
        Denied,
        Recheck,
        Complete,
        ReApproved,
        InProgress,
        Rejected,

    }
}
