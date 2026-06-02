namespace TrapchumApp.Services;

public interface IProfilePhotoService
{
    /// <summary>Raised when a user's profile photo has been updated. Payload is the new photo URL.</summary>
    event Action<string, string?> OnPhotoChanged;

    /// <summary>Notify subscribers that <paramref name="userId"/>'s photo changed to <paramref name="newUrl"/>.</summary>
    void NotifyPhotoChanged(string userId, string? newUrl);
}

public sealed class ProfilePhotoService : IProfilePhotoService
{
    public event Action<string, string?>? OnPhotoChanged;

    public void NotifyPhotoChanged(string userId, string? newUrl) =>
        OnPhotoChanged?.Invoke(userId, newUrl);
}
