namespace TrapchumApp.Shared;

public interface IAppStateService
{
    bool State { get; set; }
    bool IsAdmin { get; set; }
    bool IsAccount { get; set; }
    bool IsCreditAgent { get; set; }
    bool CanDisbursed { get; set; }
    bool CanApproved { get; set; }
    bool CanRequest { get; set; }
}