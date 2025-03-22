using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class Index
    {
        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        void SelectedTabChanged(int index)
        {
            selectedIndex = index;
        }
        RadzenTabs tabs;
        List<int> tabCounts = [0];
        protected int totalCstomers;
        int tabIndex = 1;
        int selectedIndex = 0;
        bool isNewTab = false;
        bool isDetailsTab = false;
        bool isEditTab = false;
        long customerId = 0;
        bool isVisble = false;
        bool isCollateralTab = false;
        bool IsLoading = true;
        void Details(long id)
        {
            isVisble = true;
            if (!isDetailsTab)
            {
                isDetailsTab = true;
                tabCounts.Add(2);
            }
            customerId = id;
            tabs.Reload();
            selectedIndex = 2;
            isVisble = false;
        }

        void AddCollateralTab(long id)
        {
            isVisble = true;
            if (!isCollateralTab)
            {
                isCollateralTab = true;
                tabCounts.Add(4);
            }
            customerId = id;
            tabs.Reload();
            selectedIndex = 4;
            isVisble = false;
        }

        void AddEditTab(long id)
        {
            isVisble = true;
            if (!isEditTab)
            {
                isEditTab = true;
                tabCounts.Add(3);
            }
            customerId = id;
            tabs.Reload();
            selectedIndex = 3;
            isVisble = false;
        }
        void AddCustomerTab()
        {
            if (!isNewTab)
            {
                isNewTab = true;
                tabCounts.Add(1);
            }
            tabs.Reload();
            selectedIndex = 1;
        }
        void CloseTabe()
        {
            if (selectedIndex == 1)
            {
                isNewTab = false;
                tabCounts.Remove(1);
                selectedIndex = tabCounts.Count - 1;
            }
            if (selectedIndex == 2)
            {
                isDetailsTab = false;
                tabCounts.Remove(2);
                selectedIndex = tabCounts.Count - 1;
            }
            if (selectedIndex == 3)
            {
                isEditTab = false;
                tabCounts.Remove(3);
                selectedIndex = tabCounts.Count - 1;
            }
            if (selectedIndex == 4)
            {
                isCollateralTab = false;
                tabCounts.Remove(4);
                selectedIndex = tabCounts.Count - 1;
            }
        }

    }
}