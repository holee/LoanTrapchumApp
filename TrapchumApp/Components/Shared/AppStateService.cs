namespace TrapchumApp.Components.Shared
{
    public sealed class AppStateService : IAppStateService
    {
        public bool State { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAccount { get; set; }
        public bool IsCreditAgent { get; set; } 
        public bool CanDisbursed { get; set; }
        public bool CanApproved { get; set; } 
        public bool CanRequest { get; set; }
    }
}