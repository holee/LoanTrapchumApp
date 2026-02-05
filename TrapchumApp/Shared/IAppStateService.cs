using Applications.Features.Notifications.Dto;

namespace TrapchumApp.Shared;

public interface IAppStateService
{
    bool State { get; protected set; }
    bool IsAdmin { get; protected set; }
    bool IsAccount { get; protected set; }
    bool IsCreditAgent { get; protected set; }
    bool CanDisbursed { get; protected set; }
    bool CanApproved { get; protected set; }
    bool CanRequest { get; protected set; }
    bool CanRecievePayment { get; protected set; }
    bool CanWriteOff { get; protected set; }
    bool CanOpenBranch { get; protected set; } 
}