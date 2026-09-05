namespace Web.UI.Services;

/// <summary>Notifies subscribers when a user's profile photo changes.</summary>
public interface IProfilePhotoService
{
    event Action<string, string?> OnPhotoChanged;
    void NotifyPhotoChanged(string userId, string? newUrl);
}

public sealed class ProfilePhotoService : IProfilePhotoService
{
    public event Action<string, string?>? OnPhotoChanged;

    public void NotifyPhotoChanged(string userId, string? newUrl) => OnPhotoChanged?.Invoke(userId, newUrl);
}
