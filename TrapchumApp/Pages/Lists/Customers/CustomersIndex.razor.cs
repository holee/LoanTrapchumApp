using Applications.Features.Customers.Interfaces;
using Applications.Responses.Paginations;
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
            isVisble = true;

            if (!isDetailsTab)
            {
                isDetailsTab = true;
            }
            customerId = id;

            selectedIndex = 2;

            isVisble = false;
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
            isVisble = true;


            customerId = id;


            if (!isEditTab)
            {
                isEditTab = true;
            }

            selectedIndex = 3;

            isVisble = false;
        }
        void AddCustomerTab()
        {
            if (!isNewTab)
            {
                isNewTab = true;
            }
            selectedIndex = 1;
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
                 isEditTab= false;
            }

            selectedIndex = 0;

        }

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

    }
}