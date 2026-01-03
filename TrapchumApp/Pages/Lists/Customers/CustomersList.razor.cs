using Applications.Extensions.DateTimes;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Features.Customers.Response;
using Applications.Features.Customers.UseCases;
using Applications.Features.LoanRequests.Command.IsCustomerBusy;
using Applications.Responses.Lists.Customers;
using Applications.Responses.Paginations;
using Domains.Models.Identity;
using TrapchumApp.Helpers.Enums;
using TrapchumApp.Helpers.Options;
using TrapchumApp.Pages.Lists.Collaterals;
using TrapchumApp.Pages.Lists.Customers.Expenses;
using TrapchumApp.Pages.Lists.Customers.Incomes;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomersList
    {

        private readonly GetCustomersUseCase _getCustomerCommand;

        public CustomersList(GetCustomersUseCase getCustomerCommand)
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
        [Parameter] public EventCallback<VCustomerDetails> SelectedRowChanged { get; set; }
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

        private string? _userId = string.Empty;
        void OnMenuItemClick(MenuItemEventArgs args)
        {
            ContextMenuService.Close();
        }

        IList<VCustomerDetails> SelectedCustomers = [];

        bool isLoading = false;

        bool CanRequestLoan = false;

        async Task PageSizeChanged(int page)
        {
            pageSize = page;

            pageIndex = 0; // Reset to first page when page size changes

            await FIlterCustomers(selectedValue);
        }

        private async Task FIlterCustomers(int? value)
        {
            if (value == (int)CustomerFilter.All)
            {
                await LoadCustomerPageList(0, null, filterText);
            }

            if (value == (int)CustomerFilter.MyCustomer)
            {
                await LoadCustomerPageList(null, _userId, filterText);
            }
            if (value == (int)CustomerFilter.ByBranch)
            {
                await LoadCustomerPageList(BranchId, null, filterText);
            }
        }

        async Task SelectedItemChanged(object? e)
        {
            pageIndex = 0;

            if (e is not null)
            {
                var id = ((ListItem)e).Id;

                await FIlterCustomers(id);
            }
        }




        private async Task AddInComeModal(string? customerId)
        {
            await DialogService.OpenAsync<IncomeCreate>("Income", options: new DialogOptions
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
                { "CollateralId", customerId}
            };
            var options = new DialogOptions
            {
                AutoFocusFirstElement = false
            };

            await DialogService.OpenAsync<CustomerCollateralLineCreate>("Collateral", param, options);
        }

        private async Task AddLoanRequest(string? customerId)
        {
            ContextMenuService.Close();

            var query = new IsCustomerBusyQuery(customerId);

            var rsp = await mediator.Send(query);

            if (rsp.IsError == false && rsp.Value)
            {

                NotificationService.Notify(NotificationSeverity.Error, "This customer cannot request loan again.");

                return;
            }

            var uri = $"app/loan/request/{BranchId}/branches/createNew/{customerId}/customer";

            NavigationManager.NavigateTo(uri);
        }

        private async void AddExpenseModal(string? customerId)
        {
            await DialogService.OpenAsync<CreateExpense>("Add Expense", options: new DialogOptions
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

                await LoadCustomerPageList(null, null, filterText);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

        }

        async Task SearchCustomer(string? searchText = null)
        {
            filterText = searchText ?? string.Empty;

            await LoadCustomerPageList(null, null, filterText);
        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            _userId = auth.User.GetId();

            CanRequestLoan = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.RequestLoanRequest);

            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.AccountOnly);

            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.AdminOnly);

            await LoadCustomerPageList(null, null, filterText);

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

            var filterColumn = args.Filters?.FirstOrDefault(f => f.Property == nameof(VCustomerDetails.FullName) ||
                                                            f.Property == nameof(VCustomerDetails.AltName) ||
                                                           f.Property == nameof(VCustomerDetails.CustomerId) ||
                                                           f.Property == nameof(VCustomerDetails.Phone))?.FilterValue?.ToString();

            filterText = filterColumn ?? string.Empty;

            await FIlterCustomers(selectedValue);
        }


        async Task AddCustomerTab(int selectedIndex)
        {
            ContextMenuService.Close();

            await ClickedAdd.InvokeAsync(selectedIndex);
        }

        async Task LoadCustomerPageList(int? branchId = null, string? userId = null, string? filter = null)
        {
            try
            {
                isLoading = true;

                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);

                var res = await _getCustomerCommand.ExecuteAsync(branchId, userId, null, param);

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

        void OnRowDoubleClick(DataGridRowMouseEventArgs<VCustomerList> args)
        {
            // Handle row double click event
            _ = AddDetails(args.Data.CustomerId);

            // await ClickedDetails.InvokeAsync(customerId);
        }

        void OnRowDoubleClick(VCustomerList args)
        {
            // Handle row double click event
            _ = AddDetails(args.CustomerId);
            // await ClickedDetails.InvokeAsync(customerId);
        }

        private async Task OnChanged(bool arg)
        {
            includeInactive = arg;

            await LoadCustomerPageList(0, null, filterText);
        }
    }
}