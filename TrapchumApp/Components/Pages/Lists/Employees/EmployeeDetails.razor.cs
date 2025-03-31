using Applications.Requests.Lists.Employees;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.Employees
{
    public partial class EmployeeDetails
    {
        string? imageFile = "";
        [Parameter]
        public EventCallback Close { get; set; }
        [Parameter]
        public EventCallback Edit { get; set; }
        [Parameter]
        public EmployeeDto? Model { get; set; } = new();
        async Task CloseTab()
        {
            await Close.InvokeAsync();
        }
        async Task EditTab()
        {
            await Edit.InvokeAsync();
        }
    }
}