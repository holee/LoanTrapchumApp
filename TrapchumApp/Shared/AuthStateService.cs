namespace TrapchumApp.Shared
{
    public class AuthStateService : IAuthStateService
    {
        public bool IsAuthenticated { get; set; }
        public bool Can { get; set; }
    }
}
