using Applications.Features.Employees.Queries.GetEmployeeById;
using Applications.Features.Loans.Responses;
using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Loans.Requests;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class EmployeeDetails
    {

        string? imageFile = "";
        [Parameter] public string? EmployeeId { get; set; }
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public EventCallback Close { get; set; }
        [Parameter] public EventCallback<string> OnEdit { get; set; }
        VEmployeeList? Model { get; set; } = new();

        IEnumerable<VLoanRequests> LoanRequests = [];

        IEnumerable<VLoanDisbursments> LoanDisbursments = [];
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

            var query = new GetEmployeeDetailsByIdQuery(EmployeeId, null);

            var rsp = await mediator.Send(query);

            if (rsp.IsSuccess())
            {
                Model = rsp.Value;
                LoanRequests = rsp.Value.LoanRequests ?? [];
                LoanDisbursments = rsp.Value.LoanDisbursments ?? [];
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
            await base.OnInitializedAsync();
        }
    }
}