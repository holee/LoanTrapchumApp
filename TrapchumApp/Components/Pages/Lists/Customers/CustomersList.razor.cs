using Applications.ApplicationPolicies;
using Applications.Enumerations;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Responses.Lists.Customers;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Options;
using Applications.Services.Customers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomersList
    {
        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public EventCallback<Guid?> ClickedDetails { get; set; }
        [Parameter] public EventCallback<Guid?> ClickedEdit { get; set; }
        [Parameter] public EventCallback<Guid> ClickedDelete { get; set; }
        [Parameter] public EventCallback<Guid?> ClickedAddCollateral { get; set; }
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
        long userId = 0;


        void OnMenuItemClick(MenuItemEventArgs args)
        {
            ContextMenuService.Close();
        }

        IList<VCustomerList> SelectedCustomers = [];
        bool isLoading = false;
        bool CanRequestLoan = false;
        IList<Items> pages = new Items[]
           {
        new Items(10,"10"),
        new Items(25,"25"),
        new Items(50,"50"),
        new Items(100,"100"),
           };

        void ChangedPageSize(int page)
        {
            pageSize = page;
        }

        protected async void AddInComeModal(Guid? customerId)
        {
            await DialogService.OpenAsync<CustomerModalIncome>("Income", options: new Radzen.DialogOptions
            {
                Width = "50rem",
                Height = "25rem",
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId",customerId }
            });
        }
        protected async Task AddCollateral(Guid? customerId)
        {
            ContextMenuService.Close();
            await ClickedAddCollateral.InvokeAsync(customerId);

        }
        protected void AddLoanRequest(Guid? customerId)
        {
            ContextMenuService.Close();
            var uri = $"/Loan/{customerId}/Customer/Request?BranchId={branchId}&UserId={userId}&ActionMode={ActionMode.Create}";
            NavigationManager.NavigateTo(uri);

        }
        protected async void AddExpenseModal(Guid? customerId)
        {
            await DialogService.OpenAsync<CustomerModalExpense>("Add Expense", options: new Radzen.DialogOptions
            {
                Width = "60rem",
                Height = "25rem",
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId",customerId }
            });
        }
        protected async void AddDetails(Guid? customerId)
        {
            await ClickedDetails.InvokeAsync(customerId);

            NavigationManager.NavigateTo($"/App/Customers/{customerId}/Details");

            ContextMenuService.Close();
        }
        protected async void AddEdit(Guid? customerId)
        {
            await ClickedEdit.InvokeAsync(customerId);
            ContextMenuService.Close();
        }
        protected async void AddDelete(Guid? customerId)
        {
            var confirm = await DialogService.Confirm("do you want to delete this customer?", "Delete", options: new ConfirmOptions
            {
                OkButtonText = "Yes",
                CancelButtonText = "No"
            });
            if (confirm == true)
            {

            }
        }

        async void SearchCustomer(string? searchText = null)
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
            //branchId = auth.User.GetBranchId();
            //userId = auth.User.GetId();
            CanRequestLoan = await _authorizeService.MustHasPermissionAsync(auth.User, LoanRequestsPolicies.Request);
            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.AccountOnly);
            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.AdminOnly);
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