namespace TrapchumApp.Components.Pages.Shared
{
    public interface IAuthStateService
    {
        bool IsAuthenticated { get; set; }
        bool Can { get; set; }
    }
}
