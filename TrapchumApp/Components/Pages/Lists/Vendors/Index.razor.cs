using Applications.Extensions;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Vendors;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Vendors
{
    public partial class Index
    {
        readonly IVendorService _vendorService;
        readonly IOtherNameService _otherNameService;
        public Index(IVendorService vendorService, IOtherNameService otherNameService)
        {
            _vendorService = vendorService;
            _otherNameService = otherNameService;
        }

        [Parameter] public int selectedIndex { get; set; } = 0;
        RadzenTabs? tabs;
        bool IsEnableEdit = false;
        bool IsEnableNew = false;
        bool IsEnableDetail = false;
        string? q;
        long NameId { get; set; }
        IEnumerable<ProvinceDto>? ProvinceList = [];
        EntNameDtoUpdate? model = new();
        IEnumerable<CommuneDto>? CommuneList = [];
        IEnumerable<DistrictDto>? DistrictList = [];
        IEnumerable<VillageDto>? VillageList = [];
        EntNameDto? OtherNames = new();
        ProvinceDto? SelectedProvince;
        DistrictDto? SelectedDistrict;
        CommuneDto? SelectedCommune;
        VillageDto? SelectedVillage;
        IEnumerable<VVendor> Vendors = [];
        int pageIndex = 0;
        int pageSize = 10;
        int counts = 0;
        string sortField = "EntId";
        bool sortDescending = false;
        bool includeInactive = false;
        bool isLoading = false;
        Guid? vendorId = Guid.Empty;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetVendors();
                ProvinceList = await _addressService!.GetsProvinces();
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, "Exception");
            }
            await base.OnInitializedAsync();

        }


        protected async Task OnSearch(ChangeEventArgs e)
        {
            q = e.Value!.ToString();
            await GetVendors(q);
        }

        async Task PageSizeChanged(int page)
        {
            pageSize = page;
            await GetVendors(q);
        }

        async Task GetVendors(string? filterText = "")
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filterText);

            var resp = await _vendorService.GetVendorsAsync(param);

            counts = resp.Count;

            Vendors = resp.Items;

            isLoading = false;

            StateHasChanged();
        }

        async Task LoadData(LoadDataArgs e)
        {
            q = e.Filter ?? string.Empty;
            pageIndex = e.Skip!.Value;
            if (!string.IsNullOrEmpty(e.OrderBy))
            {
                sortField = e.OrderBy.Split(" ")[0];
                string dir = e.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }
            await GetVendors(q);

        }
        void ShowEdit(Guid? id)
        {
            NavigationManager.NavigateTo($"/App/Vendors/{id}/Edit");
        }

        void SelectedId(Guid? id)
        {
            vendorId = id;
        }

        protected async Task DisplayDetail(Guid? e)
        {
            if (e.IsNotNullOrEmpty())
            {
                try
                {
                    var name = await _otherNameService!.GetByIdAsync(e);

                    OtherNames = name.Adapt<EntNameDto>();

                    IsEnableDetail = true;

                    selectedIndex = 3;

                    await InvokeAsync(StateHasChanged);

                    ContextMenuService.Close();

                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
                }

            }
        }
        async Task OnDelete(Guid? id)
        {
            ContextMenuService.Close();
            var confirm = (bool)await DialogService.Confirm("Other Name deleted will not be able to recover, would you like to continue?", "Delete Other Name",
        new Radzen.ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            if (!confirm)
            {
                return;
            }
            var result = await _otherNameService!.DeleteAsync(id);
            if (result.IsSuccess)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Success, "Delete Other Name", "Other Name was deleted successful!");
                await GetVendors(q);
            }

        }
        protected async Task IncludeInActiveChanged(bool e)
        {
            includeInactive = e;
            await GetVendors(q);
        }
        protected async Task Reload()
        {
            await GetVendors(q);
        }




        void AddTabOtherName()
        {
            IsEnableNew = true;
            selectedIndex = 1;
            StateHasChanged();
        }
        async void CloseTabs(int e)
        {
            if (selectedIndex == 1)
            {
                IsEnableNew = false;
                selectedIndex = 0;

                await OnInitializedAsync();
            }
            else if (selectedIndex == 2)
            {
                IsEnableEdit = false;
                selectedIndex = 0;

                await OnInitializedAsync();
            }
            else if (selectedIndex == 3)
            {
                IsEnableDetail = false;
                selectedIndex = 0;

            }
            StateHasChanged();
        }

        protected async Task DeactiveOrActive(bool arg, VVendor e)
        {
            var entVendor = new EntVendorDtoChangeStatus
            {
                VendorId = e.EntId,
                EditSeq = e.EditSeq,
                IsActive = arg
            };
            var response = await _vendorService.ChangeVendorStatus(entVendor);
            if (response.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "", response.Data);
                await GetVendors(q);
            }
            else
            {
                await IncludeInActiveChanged(false);
                NotificationService.Notify(NotificationSeverity.Warning, "", response.ErrMessage);
            }
        }
    }
}