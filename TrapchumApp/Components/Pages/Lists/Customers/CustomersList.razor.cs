using Applications.ApplicationPolicies;
using Applications.Enumerations;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Responses.Lists.Customers;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomersList
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public EventCallback<string?> ClickedDetails { get; set; }
        [Parameter] public EventCallback<string?> ClickedEdit { get; set; }
        [Parameter] public EventCallback<string> ClickedDelete { get; set; }
        [Parameter] public EventCallback<string?> ClickedAddCollateral { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; } = string.Empty;
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public EventCallback<VCustomerList> SelectedRowChanged { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; } = default!;

        bool IsAccountOnly = false;

        bool IsAdminOnly = false;

        protected CustomerResponse? Customer;

        protected List<VCustomerList>? VCustomers = [];

        protected bool includeInactive = false;

        protected string SearchTerm = string.Empty;

        private int counts = 0;

        private int pageSize = 10;

        int branchId = 0;

        void OnMenuItemClick(MenuItemEventArgs args)
        {
            ContextMenuService.Close();
        }

        IList<VCustomerList> SelectedCustomers = [];

        bool isLoading = false;

        bool CanRequestLoan = false;

        void ChangedPageSize(int page)
        {
            pageSize = page;
        }

        private async Task AddInComeModal(string? customerId)
        {
            await DialogService.OpenAsync<CustomerModalIncome>("Income", options: new DialogOptions
            { 
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId", customerId }
            });
        }

        protected async Task AddCollateral(string? customerId)
        {
            ContextMenuService.Close();
            await ClickedAddCollateral.InvokeAsync(customerId);
        }

        private void AddLoanRequest(string? customerId)
        {
            ContextMenuService.Close();
            var uri =$"/App/Request/{BranchId}/Branches/{UserId}/CreateNew?CustomerId={customerId}";
            NavigationManager.NavigateTo(uri);
        }

        private async void AddExpenseModal(string? customerId)
        {
            await DialogService.OpenAsync<CustomerModalExpense>("Add Expense", options: new DialogOptions
            {
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId", customerId }
            });
        }

        private void AddDetails(string? customerId)
        {
            ContextMenuService.Close();

            //await ClickedDetails.InvokeAsync(customerId);

            NavigationManager.NavigateTo($"/App/Customers/{customerId}/Details?branchId={BranchId}");

            
        }

        private async void AddEdit(string? customerId)
        {
            await ClickedEdit.InvokeAsync(customerId);

            ContextMenuService.Close();
        }

        private async void AddDelete(string? customerId)
        {
            var confirm = await DialogService.Confirm("do you want to delete this customer?", "Delete",
                options: new ConfirmOptions
                {
                    OkButtonText = "Yes",
                    CancelButtonText = "No"
                });
            if (confirm == true)
            {
            }
        }

        async Task SearchCustomer(string? searchText = null)
        {
            SearchTerm = searchText ?? string.Empty;

            await LoadCustomerPageList(); 
        }

        async Task SelectedPageChanged(int pageSize)
        {
            this.pageSize = pageSize;
            await LoadCustomerPageList();
        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            CanRequestLoan = await _authorizeService.MustHasPermissionAsync(auth.User, RequestsPolicies.Request);
            
            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(auth.User, RolePolicies.AccountOnly);
            
            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(auth.User, RolePolicies.AdminOnly);
            
            await LoadCustomerPageList();
           
            await base.OnInitializedAsync();
        }

        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "CustomerId";
            bool sortDescending = false;
            int pageIndex = args.Skip!.Value;
            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortField = args.OrderBy.Split(" ")[0];
                string dir = args.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }

            await LoadCustomerPageList(pageIndex, sortField, sortDescending);
        }

        async Task LoadCustomerPageList(int pageIndex = 0, string sortField = "CustomerId", bool sortDescending = false)
        {
            try
            {
                isLoading = true;

                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, SearchTerm);

                var res = await _customerService.GetCustomers(param);

                VCustomers = res.Items;
                counts = res.Count;
                isLoading = false;
                StateHasChanged();
            }
            catch
            {
                isLoading = false;
            }
        }

        void OnRowRender(RowRenderEventArgs<VCustomerList> e)
        {
            if (e.Data.IsLoanActive)
            {
                e.Attributes.Add("style", "font-weight:bold;color:red;background-color:yellow;");
            }
        }
    }
}