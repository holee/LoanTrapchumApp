using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class Guarantors
    {
        private readonly IGuarantorServiceImpl _gService;
        public Guarantors(IGuarantorServiceImpl gService)
        {
            _gService = gService;
        }

        bool includeInactive = true;
        string? filterText = "";
        int pageIndex = 0;
        int pageSize = 10;
        string sortColumn = "FullName";
        bool sortDescending = false;
        int counts = 0;
        bool isLoading = false;
        bool ShowAddTab = false;
        bool ShowDetailTab = false;
        bool ShowEditTab = false;
        RadzenTabs tabs = default!;
        int selectedTabIndex = 0;
        string? SelectedGuarantorId = string.Empty;
        IEnumerable<VGuarantor> guarantors = [];
        EntityNamesCreate modelCreate = new EntityNamesCreate();
        EntityNamesCreate modelEdit = new EntityNamesCreate();

        void AddGuarantorTab(int index)
        {
            if (!ShowAddTab)
            {
                ShowAddTab = true;

                modelCreate.Gender = "mr";
            }

            selectedTabIndex = index;

        }

        void EditGuarantorTab(int index)
        {
            if (!ShowEditTab)
            {
                ShowEditTab = true;
            }

            selectedTabIndex = index;

        }

        void DetailGuarantorTab(int index)
        {
            if (!ShowDetailTab)
            {
                ShowDetailTab = true;
            }

            selectedTabIndex = index;

        }

        async Task SelectTabChanged(int index)
        {
            if (index == 1)
            {
                await GetGuarantors();
            }

        }

        void CloseGuarantorTab(int tabIndex)
        {
            if (tabIndex == 1 && ShowAddTab)
            {
                ShowAddTab = false;
            }
            if (tabIndex == 2 && ShowEditTab)
            {
                ShowEditTab = false;
            }
            if (tabIndex == 3 && ShowDetailTab)
            {
                ShowDetailTab = false;
            }
            selectedTabIndex = 0;
        }

        void SetGuarantorId(string guarantorId)
        {
            SelectedGuarantorId = guarantorId;
        }

        async Task GetGuarantors()
        {
            isLoading = true;
            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, includeInactive, filterText);
            var guarantorResp = await _gService.GetGuarantorsAsync(param);
            if (guarantorResp.IsSuccess)
            {
                guarantors = guarantorResp.Data.Items;

                counts = guarantorResp.Data.Count;
            }
            isLoading = false;

            StateHasChanged();
        }

    }
}