using Microsoft.AspNetCore.Components;
using System.Text.Json;
using TrapchumApp.Components.Shared;

namespace TrapchumApp.Components.Pages.Shared
{
    public partial class CascadingAuthState : ComponentBase, IAuthStateService
    {
        private bool _isAuthencated;
        private bool _can;
        public bool IsAuthenticated
        {
            get => _isAuthencated;
            set
            {
                _isAuthencated = value;
                StateHasChanged();
            }
        }
        public bool Can
        {
            get => _can;
            set
            {
                _can = value;
                StateHasChanged();
            }
        }


        public IAuthStateService? CopyState()
        {
            var state = (IAuthStateService)this;
            var json = JsonSerializer.Serialize(state);
            var copy = JsonSerializer.Deserialize<AuthStateService>(json);
            return copy;
        }
    }
}
