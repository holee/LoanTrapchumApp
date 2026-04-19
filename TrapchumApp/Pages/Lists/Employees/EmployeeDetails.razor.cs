using Applications.Features.Employees.Queries.GetEmployeeById;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Loans.Requests;
using TrapchumApp.Helpers.Extensions;
using System.Globalization;

namespace TrapchumApp.Pages.Lists.Employees;

public partial class EmployeeDetails
{

    string? imageFile = "";
    [Parameter] public string? EmployeeId { get; set; }
    [Parameter] public int? BranchId { get; set; } = 0;
    [Parameter] public EventCallback Close { get; set; }
    [Parameter] public EventCallback<string> OnEdit { get; set; }

    VEmployeeList? Model = new();

    IEnumerable<VLoanRequests> LoanRequests = [];

    IEnumerable<VLoanDisbursments> LoanDisbursments = [];

    private bool _isLoading = false;

    private const string EmptyValue = "Not provided";

    private bool HasPhoto => !string.IsNullOrWhiteSpace(Model?.Photo);

    private string EmploymentStatus => Model?.IsActive == true ? "Active" : "Inactive";

    private string AvatarInitials
    {
        get
        {
            var first = !string.IsNullOrWhiteSpace(Model?.FirstName) ? Model!.FirstName!.Trim()[0].ToString() : string.Empty;
            var last = !string.IsNullOrWhiteSpace(Model?.LastName) ? Model!.LastName!.Trim()[0].ToString() : string.Empty;
            var initials = string.Concat(first, last);

            if (!string.IsNullOrWhiteSpace(initials))
            {
                return initials.ToUpperInvariant();
            }

            if (!string.IsNullOrWhiteSpace(Model?.FullName))
            {
                return Model.FullName.Trim()[0].ToString().ToUpperInvariant();
            }

            return "NA";
        }
    }

    private string AddressDisplay
    {
        get
        {
            var parts = new[]
            {
                Model?.VilInKhmer,
                Model?.ComInKhmer,
                Model?.DisInKhmer,
                Model?.City,
                Model?.CountryName
            };

            var address = string.Join(", ", parts.Where(static part => !string.IsNullOrWhiteSpace(part)));
            return string.IsNullOrWhiteSpace(address) ? EmptyValue : address;
        }
    }

    async Task CloseTab()
    {
        await Close.InvokeAsync();
    }
    async Task EditTab()
    {
        await OnEdit.InvokeAsync(EmployeeId);
    }

    protected override async Task OnInitializedAsync()
    {
        await GetEmployees();

        await base.OnInitializedAsync();
    }

    private async Task GetEmployees()
    {
        Tracker.Show();

        _isLoading = true;

        var query = new GetEmployeeDetailsByIdQuery(EmployeeId, null);

        var rsp = await mediator.Send(query);

        Model = rsp.Value;
        
        Model.Photo ??= "/assets/profiles/profile.png";

        LoanRequests = rsp.Value.LoanRequests ?? [];

        LoanDisbursments = rsp.Value.LoanDisbursments ?? [];

        Tracker.Hide();

        _isLoading = false;

        StateHasChanged();
    }

    private static string DisplayOrFallback(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? EmptyValue : value;
    }

    private static string FormatDate(DateTime? date)
    {
        return date.HasValue
            ? date.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)
            : EmptyValue;
    }
}
