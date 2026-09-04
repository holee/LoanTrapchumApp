using Applications.Bases;
using Applications.Extensions;
using Applications.Extensions.Attrs;
using Applications.Features.Customers.Commands.DeleteCustomer;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Applications.Features.LoanRequests.Command.IsCustomerBusy;
using Applications.Responses.Lists.Customers;
using Domains.Models.Identity;
using Web.UI.Helpers.Enums;
using Web.UI.Helpers.Options;
using Web.UI.Pages.Lists.Collaterals;
using Web.UI.Pages.Lists.Customers.Expenses;
using Web.UI.Pages.Lists.Customers.Incomes;
using Web.UI.Shared;

namespace Web.UI.Pages.Lists.Customers
{
    public partial class CustomersIndex
    {

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] CascadingAppState BState { get; set; } = default!;
        [Parameter] public int? BranchId { get; set; }
        private List<CustomerTab> _tabs = new();
        protected int totalCstomers;
        private int selectedIndex = 0;
        private string? customerId = string.Empty;
        [PersistentState] public List<VCustomerList>? VCustomers { get; set; }

        private bool includeInactive = false;

        private string? filterText = string.Empty;

        private int counts = 0;

        private int pageSize = 15;

        private int pageIndex = 0;

        private string sortField = nameof(VCustomerList.ModifiedDT);

        private bool sortDescending = true;

        private IList<VCustomerDetails> SelectedCustomers = [];

        private string? _userId = string.Empty;

        private string? _lastRowId = string.Empty;


        private readonly string[] filterableProperties = new[]
        {
            nameof(VCustomerDetails.FullName),
            nameof(VCustomerDetails.AltName),
            nameof(VCustomerDetails.CustomerId),
            nameof(VCustomerDetails.Phone)
        };

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationState;

            var user = authState.User;

            _userId = user.GetId();

            await BState.GetAuthenticatedUser(user);

            if (VCustomers is null)
            {
                await LoadCustomerPageList(null, null, filterText);
            }

            await base.OnInitializedAsync();
        }


        void SelectedTabChanged(int index)
        {
            selectedIndex = index;
        }

        async Task SearchCustomer(string? searchText = null)
        {
            filterText = searchText ?? string.Empty;

            await FilterCustomers(selectedValue);
        }

        private async Task OnChanged(bool arg)
        {
            includeInactive = arg;

            await FilterCustomers(selectedValue);
        }

        protected async Task AddCollateral(string? customerId)
        {
            ContextMenuService.Close();

            var param = new Dictionary<string, object?>
            {
                { "CollateralId", customerId},
                { "PageType", PageType.Dialog }
            };
            var options = new DialogOptions
            {
                AutoFocusFirstElement = false,
                Width = "100vw",

            };

            await DialogService.OpenAsync<CustomerCollateralLineCreate>("Collateral", param, options);
        }

        private async Task AddInComeModal(string? customerId)
        {
            await DialogService.OpenAsync<IncomeCreate>("Income", options: new DialogOptions
            {
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId", customerId! }
            });
        }


        private async void AddExpenseModal(string? customerId)
        {
            await DialogService.OpenAsync<CreateExpense>("Add Expense", options: new DialogOptions
            {
                Resizable = false,
                Draggable = true,
            }, parameters: new Dictionary<string, object?>
            {
                { "CustomerId", customerId! }
            });
        }


        private async Task AddDetails(string? customerId)
        {
            ContextMenuService.Close();

            Details(customerId);
        }

        private async Task AddEdit(string? customerId)
        {
            ContextMenuService.Close();

            AddEditTab(customerId);
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

            var rsp = await mediator.Send(new DeleteCustomerCommand(customerId));  //_customerService.DeleteCustomerAsync(customerId);

            if (rsp.Status == DeleteStatus.Success)
            {
                _toastService.ShowSuccess(rsp.Message, "Delete Success");

                await LoadCustomerPageList(null, null, filterText);

                return;
            }

            if (rsp.Status == DeleteStatus.NotFound)
            {
                _toastService.ShowError(rsp.Message, "Delete Failed");

                return;
            }

            if (rsp.Status == DeleteStatus.InUse)
            {

                _toastService.ShowError(rsp.Message, "Delete Failed");

                return;
            }

        }



        void Details(string? id)
        {

            var existingTabIndex = _tabs.FindIndex(t => t.TabId == id);

            if (existingTabIndex != -1)
            {
                selectedIndex = existingTabIndex + 1;

                StateHasChanged();

                return;
            }

            RenderFragment childContent = builer =>
            {
                builer.OpenComponent(0, typeof(CustomerDetailsPage));
                builer.AddAttribute(1, "BranchId", BranchId);
                builer.AddAttribute(2, "CustomerId", id);
                builer.AddAttribute(3, "Closed", EventCallback.Factory.Create(this, () => CloseTab(id)));
                builer.AddAttribute(4, "EditClick", EventCallback.Factory.Create(this, async (string x) => AddEditTab(id)));
                builer.CloseComponent();

            };

            var newTab = new CustomerTab
            {
                TabId = id ?? string.Empty,

                Title = "Details",

                CustomerAction = CustomerAction.Details,

                ChildContent = childContent,
            };

            _tabs.Add(newTab);

            selectedIndex = _tabs.Count;
        }

        void AddEditTab(string? id)
        {
            var editTabId = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString("N") : $"{id}_edit";

            var existingTabIndex = _tabs.FindIndex(t => t.TabId == editTabId);

            if (existingTabIndex != -1)
            {
                selectedIndex = existingTabIndex + 1;

                return;
            }

            RenderFragment childContent = builer =>
            {
                builer.OpenComponent(0, typeof(CustomerUpdatePage));

                builer.AddAttribute(1, "CustomerId", id);

                builer.AddAttribute(2, "Closed", EventCallback.Factory.Create(this, () =>
                {
                    CloseTab(editTabId);
                }));

                builer.AddAttribute(3, "UpdateChanged", EventCallback.Factory.Create(this, async (VCustomerList x) =>
                {

                    var foundIndex = VCustomers?.FindIndex(c => c.CustomerId == x.CustomerId) ?? -1;

                    if (foundIndex != -1)
                    {
                        VCustomers![foundIndex] = x;
                    }

                    await _gridCustomer.Reload();

                }));

                builer.AddAttribute(4, "BranchId", BranchId);

                builer.SetKey(editTabId);

                builer.CloseComponent();
            };


            var newTab = new CustomerTab
            {
                TabId = editTabId,

                Title = "Edit Customer",

                CustomerAction = CustomerAction.Update,

                ChildContent = childContent,
            };

            _tabs.Add(newTab);

            selectedIndex = _tabs.Count;


        }

        void AddCustomerTab()
        {
            var newTabId = Guid.NewGuid().ToString("N");

            RenderFragment childContent = builer =>
            {
                builer.OpenComponent(0, typeof(CustomerCreatePage));

                builer.AddAttribute(1, "Closed", EventCallback.Factory.Create(this, () =>
                 {
                     CloseTab(newTabId);
                 }));
                builer.AddAttribute(2, "UpdateState", EventCallback.Factory.Create(this, async (VCustomerList x) =>
                {
                    _lastRowId = x.CustomerId;

                    await _gridCustomer.Reload();

                }));

                builer.AddAttribute(3, "BranchId", BranchId);

                builer.SetKey(newTabId);

                builer.CloseComponent();
            };

            var newTab = new CustomerTab
            {
                TabId = newTabId,

                Title = "New Customer",

                CustomerAction = CustomerAction.Create,

                ChildContent = childContent,
            };

            _tabs.Add(newTab);

            selectedIndex = _tabs.Count;
        }

        private void CloseTab(string? id)
        {
            var tab = _tabs.FirstOrDefault(t => t.TabId == id);

            if (tab != null)
            {
                var idx = _tabs.IndexOf(tab);

                _tabs.Remove(tab);

                selectedIndex = Math.Max(0, idx);
            }

        }



        void OnRowRender(RowRenderEventArgs<VCustomerList> args)
        {
            if (args.Data is null) return;

            var searchMatching = !string.IsNullOrEmpty(filterText) &&
                                 (args.Data!.FullName?.Contains(filterText, StringComparison.OrdinalIgnoreCase) == true ||
                                  args.Data!.AltName?.Contains(filterText, StringComparison.OrdinalIgnoreCase) == true ||
                                  args.Data!.FirstName?.Contains(filterText, StringComparison.OrdinalIgnoreCase) == true ||
                                  args.Data!.LastName?.Contains(filterText, StringComparison.OrdinalIgnoreCase) == true ||
                                  args.Data!.Phone?.Contains(filterText, StringComparison.OrdinalIgnoreCase) == true);

            args.Data.RowIndex = args.Index + 1;

            if (args.Data!.IsLoanActive)
            {
                args.Attributes["style"] = "font-weight:bold;color:red;background-color:yellow;";
            }

            if (searchMatching)
            {
                args.Attributes["class"] = "rz-state-highlight rz-data-row recently-edited";
            }

            if (args.Data!.ModifiedDT.IsRecently())
            {
                args.Attributes["class"] = "rz-state-highlight rz-data-row recently-edited";
            }


        }

        void OnCellRender(RowRenderEventArgs<VCustomerList> e)
        {
            if (e.Data!.CustomerId == _lastRowId)
            {
                e.Attributes["class"] = "rz-state-highlight rz-data-row recently-edited";
            }
        }


        private void OnRowSelected(VCustomerList args)
        {

        }

        void OnRowDoubleClick(DataGridRowMouseEventArgs<VCustomerList> args)
        {
            _ = AddDetails(args.Data!.CustomerId);
        }

        void OnRowDoubleClick(VCustomerList args)
        {
            _ = AddDetails(args.CustomerId);
        }

        async Task LoadCustomerPageList(int? branchId = null, string? userId = null, string? filter = null)
        {
            try
            {
                Tracker.Show();

                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);

                var command = new GetCustomerAsPagingQuery(branchId, userId, string.Empty, param);

                var rsp = await mediator.Send(command);

                VCustomers = rsp.Items;

                counts = rsp.Count;

                Tracker.Hide();

                StateHasChanged();
            }
            catch
            {
                Tracker.Hide();
            }
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

            var uri = $"/App/Loans/Requests/{BranchId}/Branches/CreateNew/{customerId}/Customers";

            NavigationManager.NavigateTo(uri);
        }




        async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;

            pageSize = args.Top ?? pageSize;

            var sort = args.Sorts?.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(sort?.Property))
            {
                sortField = sort.Property;

                sortDescending = sort.SortOrder == SortOrder.Descending;
            }

            filterText = args.FilterValue(filterableProperties);

            await FilterCustomers(selectedValue);
        }



        private async Task FilterCustomers(int? value)
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

                await FilterCustomers(id);
            }
        }


    }
}
