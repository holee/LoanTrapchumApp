using Applications.Extensions.DateTimes;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Features.Customers.UseCases;
using Applications.Responses.Lists.Customers;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using TrapchumApp.Pages.Lists.Collaterals;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomersList
    {

        private readonly GetCustomersCommand _getCustomerCommand;

        public CustomersList(GetCustomersCommand getCustomerCommand)
        {
            _getCustomerCommand = getCustomerCommand;
        }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public EventCallback<string?> ClickedDetails { get; set; }
        [Parameter] public EventCallback<int?> ClickedAdd { get; set; }
        [Parameter] public EventCallback<string?> ClickedEdit { get; set; }
        [Parameter] public EventCallback<int> EditIndex { get; set; }
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

        protected string filterText = string.Empty;

        private int counts = 0;

        private int pageSize = 15;

        private int pageIndex = 0;

        private string sortField = "CustomerId";

        private bool sortDescending = false;

        int branchId = 0;

        void OnMenuItemClick(MenuItemEventArgs args)
        {
            ContextMenuService.Close();
        }

        IList<VCustomerList> SelectedCustomers = [];

        bool isLoading = false;

        bool CanRequestLoan = false;

        async Task PageSizeChanged(int page)
        {
            pageSize = page;

            pageIndex = 0; // Reset to first page when page size changes

            await LoadCustomerPageList(filterText);

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
            var param = new Dictionary<string, object?>
            {
                { "CustomerId", customerId}
            };
            var options = new DialogOptions
            {
                Width = "100vw",
                Height = "100vh",
                AutoFocusFirstElement = false
            };

            await DialogService.OpenAsync<CollateralModalAdd>("Collateral", param, options);
        }

        private async Task AddLoanRequest(string? customerId)
        {
            ContextMenuService.Close();

            if (await _loanRequestService.IsCustomerBusy(customerId))
            {
                NotificationService.Notify(NotificationSeverity.Error, "This customer cannot request loan again.");
                return;
            }

            var uri = $"/App/Request/{BranchId}/Branches/{UserId}/CreateNew?CustomerId={customerId}";

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

        private async Task AddDetails(string? customerId)
        {
            ContextMenuService.Close();

            await ClickedDetails.InvokeAsync(customerId);

        }

        private async Task AddEdit(string? customerId)
        {
            ContextMenuService.Close();

            await ClickedEdit.InvokeAsync(customerId);

            await EditIndex.InvokeAsync(3);

        }

        private async void AddDelete(string? customerId)
        {
            var confirm = await DialogService.Confirm("do you want to delete this customer?", "Delete",
                options: new ConfirmOptions
                {
                    OkButtonText = "Yes",
                    CancelButtonText = "No"
                });
            if (confirm == false) return;

            var rsp = await _customerService.DeleteCustomerAsync(customerId);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

                await LoadCustomerPageList(filterText);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

        }

        async Task SearchCustomer(string? searchText = null)
        {
            filterText = searchText ?? string.Empty;

            await LoadCustomerPageList(searchText);
        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            CanRequestLoan = await _authorizeService.MustHasPermissionAsync(auth.User, RequestsPolicies.Request);

            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(auth.User, RolePolicies.AccountOnly);

            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(auth.User, RolePolicies.AdminOnly);

            await LoadCustomerPageList(filterText);

            await base.OnInitializedAsync();
        }

        async Task LoadData(LoadDataArgs args)
        {
            sortField = "CustomerId";

            sortDescending = false;

            pageIndex = args.Skip!.Value;

            if (args.Sorts.Any())
            {
                sortField = args.Sorts.First().Property;

                sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
            }

            var filterColumn = args.Filters?.FirstOrDefault(f => f.Property == nameof(VCustomerList.FullName) ||
                                                            f.Property == nameof(VCustomerList.AltName) ||
                                                           f.Property == nameof(VCustomerList.CustomerId) ||
                                                           f.Property == nameof(VCustomerList.Phone))?.FilterValue?.ToString();

            filterText = filterColumn ?? string.Empty;

            await LoadCustomerPageList(filterText);
        }


        async Task AddCustomerTab(int selectedIndex)
        {
            ContextMenuService.Close();

            await ClickedAdd.InvokeAsync(selectedIndex);
        }

        async Task LoadCustomerPageList(string? filter = null)
        {
            try
            {
                isLoading = true;

                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);

                var res = await _getCustomerCommand.ExecuteAsync(null,param);

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

        void OnRowRender(RowRenderEventArgs<VCustomerList> args)
        {
            if (args.Data.IsLoanActive)
            {
                args.Attributes.Add("style", "font-weight:bold;color:red;background-color:yellow;");
            }

            if (args.Data.ModifiedDT.IsRecently())
            {

                args.Attributes.Add("class", "rz-state-highlight rz-data-row recently-edited");

            }
        }

        void OnCellRender(RowRenderEventArgs<VCustomerList> e)
        {

        }


        private void OnRowSelected(VCustomerList args)
        {

        }

        private async Task OnChanged(bool arg)
        {
            includeInactive = arg;

            await LoadCustomerPageList(filterText);
        }
    }
}