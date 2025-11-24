using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Interfaces;
using Microsoft.JSInterop;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomersIndex
    {
        private readonly IGetCustomersByUseCase _getCustomersByUseCase;

        public CustomersIndex(IGetCustomersByUseCase getCustomersByUseCase)
        {
            _getCustomersByUseCase = getCustomersByUseCase;
        }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
        [Parameter] public int? BranchId { get; set; }
        void SelectedTabChanged(int index)
        {
            selectedIndex = index;
        }
        RadzenTabs tabs = default!;
        List<int> tabCounts = [0];
        private List<CustomerTab> _tabs = new();
        protected int totalCstomers;
        int tabIndex = 1;
        int selectedIndex = 0;
        bool isNewTab = false;
        bool isDetailsTab = false;
        bool isEditTab = false;
        string? customerId = string.Empty;
        bool isVisble = false;
        bool isCollateralTab = false;
        bool IsLoading = true;

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
                builer.OpenComponent(0, typeof(CustomerDetails));
                builer.AddAttribute(1, "BranchId", BranchId);
                builer.AddAttribute(2, "CustomerId", id);
                builer.AddAttribute(3, "Closed", EventCallback.Factory.Create(this, (int x) =>
                {
                    CloseTab(id);
                }));

                builer.AddAttribute(4, "EditClick", EventCallback.Factory.Create(this, async (string x) =>
                {
                    AddEditTab(id);

                }));

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

        void AddCollateralTab(string? id)
        {
            isVisble = true;

            customerId = id;

            if (!isCollateralTab)
            {
                isCollateralTab = true;
            }

            selectedIndex = 4;

            isVisble = false;
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
                builer.OpenComponent(0, typeof(CustomerUpdateTab));
                builer.AddAttribute(1, "CustomerId", id);
                builer.AddAttribute(2, "Closed", EventCallback.Factory.Create(this, (int x) =>
                {
                    CloseTab(editTabId);
                }));

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
                builer.OpenComponent(0, typeof(CustomerNewCreate));
                builer.AddAttribute(1, "CustomerModel", new EntityCustomerCreate());
                builer.AddAttribute(2, "Closed", EventCallback.Factory.Create(this, (int x) =>
                 {
                     CloseTab(newTabId);
                 }));
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



        void CloseTabe(int closeTabIndex)
        {
            if (closeTabIndex == 1)
            {
                isNewTab = false;
            }

            if (closeTabIndex == 2)
            {
                isDetailsTab = false;
            }

            if (closeTabIndex == 3)
            {
                isEditTab = false;
            }

            selectedIndex = 0;

        }
    }
}