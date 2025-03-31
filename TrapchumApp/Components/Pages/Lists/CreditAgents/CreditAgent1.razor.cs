using Applications.Requests.Loans.CreditAgents;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TrapchumApp.Components.Pages.Accounts.Users;

namespace TrapchumApp.Components.Pages.Lists.CreditAgents
{
    public partial class CreditAgent
    {
        int BranchId;
        string? BranchName;
        [Parameter] public int Index { get; set; } = 1;
        bool isLoading = false;
        bool ShowAddedTab = false;
        RadzenTabs tabs = default!;
        LoanCreditAgentDtoCreate model = new LoanCreditAgentDtoCreate();
        IEnumerable<VEmployeeCreateCreditAgent> CreditAgentPageList = [];
        int selectedTabIndex = 0;
        private async Task AddCreditAgent(Guid? id)
        {

            bool? confirm = await DialogService.Confirm($"do you want make Employee #{id} as C.O?",
                                    "Confirm",
                                     new ConfirmOptions
                                     {
                                         OkButtonText = "Ok",
                                         CancelButtonText = "Cancel"
                                     });
            if (confirm == false) return;

            var response = await _creditAgentService.AddCreditAgent(id, model);

            if (response!.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, response.Data);
                ShowAddedTab = false;
                selectedTabIndex = 0;
                await GetCreditAgents(searchText);
                tabs.Reload();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, response.ErrMessage);
            }
        }

        void ChangedPageSize(int page)
        {
            pageSize = page;
        }




        async void SelectedTabChanged(int index)
        {
            selectedTabIndex = index;
            if (selectedTabIndex == 1)
            {
                await LoadPageList();
            }
        }

        async Task AddCreditAgentTab()
        {
            if (!ShowAddedTab)
            {
                ShowAddedTab = true;
                await LoadPageList();
            }
            selectedTabIndex = 1;
            tabs.Reload();
        }
        void RemoveTab()
        {
            if (selectedTabIndex == 1 && ShowAddedTab)
            {
                ShowAddedTab = false;
            }
            selectedTabIndex = 0;
        }

        async Task SearchCreditAgent()
        {
            await LoadPageList(searchText);
        }

        async Task LoadPageList(string? filter = null)
        {
            CreditAgentPageList = await _creditAgentService.GetEmployeeCreateCreditAgents(filter);
        }

        async Task ShowDialogue()
        {
            var options = new DialogOptions
            {
                Width = "80rem",
                Height = "30rem"
            };
            await DialogService.OpenAsync<CreditAgentDialog>("Credit Agent", options: options);
        }
        async void Reload()
        {
            await GetCreditAgents(searchText);
        }


        async Task AddUserAccount(VCreditAgent c)
        {

            var options = new DialogOptions
            {
                Width = "620px",
                ContentCssClass = "rz-mt-3 rz-background-color-base-100"
            };

            var param = new Dictionary<string, object?>
        {
            {"EmployeeId",c.EmployeeRefId},
            {"BranchId",c.BranchId}
        };
            var result = await DialogService.OpenAsync<CreditAgentUserCreate>("New User", param, options: options);

            if (bool.TryParse(result, out bool success) && success)
            {

            }
        }

    }
}