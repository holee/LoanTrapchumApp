using Applications.Features.Notifications.Dto;

namespace TrapchumApp.Shared;

public sealed class AppStateService : IAppStateService
{
    public bool State { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsAccount { get; set; }
    public bool IsCreditAgent { get; set; }
    public bool CanDisbursed { get; set; }
    public bool CanApproved { get; set; }
    public bool CanRequest { get; set; }
    public bool CanRecievePayment { get; set; }
    public bool CanWriteOff { get; set; }
    public int UnReadCount { get; set; }
    public bool CanOpenBranch { get; set; }
    public List<NotificationDto> Notifications { get; set; } = [];
    public bool CanRecieveMultiple { get; set; }
}