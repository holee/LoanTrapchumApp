using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class EmployeeDetails
    {


        string? imageFile = "";
        [Parameter] public string? EmployeeId { get; set; }
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public EventCallback<bool> Close { get; set; }
        [Parameter] public EventCallback<string> OnEdit { get; set; }
        VEmployeeList? Model { get; set; } = new();

        IEnumerable<VLoanRequests> LoanRequests = [];

        IEnumerable<VLoanDisbursments> LoanDisbursments = [];
        async Task CloseTab()
        {
            await Close.InvokeAsync(false);
        }
        async Task EditTab()
        {
            await OnEdit.InvokeAsync(EmployeeId);
        }

        protected override async Task OnInitializedAsync()
        {
            var rsp = await _employeeService.GetEmployeeById(EmployeeId);

            if (rsp.IsSuccess)
            {
                Model = rsp.Data!;
                LoanRequests = rsp.Data?.LoanRequests ?? [];
                LoanDisbursments = rsp.Data?.LoanDisbursments ?? [];
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
                await base.OnInitializedAsync();
        }
    }
}