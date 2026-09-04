namespace Web.UI.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

public class NavigationService : IDisposable
{
    private readonly NavigationManager _navigationManager;

    private string? _previousUrl;

    private string? _currentUrl;
    public string? PreviousUrl => _previousUrl;
    public string? CurrentUrl => _currentUrl;
    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        _currentUrl = _navigationManager.Uri;

        _navigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _previousUrl = _currentUrl;

        _currentUrl = e.Location;
    }


    public void GoBack()
    {
        if (!string.IsNullOrEmpty(_previousUrl))
        {
            // Navigate to the previous URL
            _navigationManager.NavigateTo(_previousUrl!);
        }
        else
        {
            // Optional: fallback to home if no previous page
            _navigationManager.NavigateTo("/");
        }
    }

    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
}
