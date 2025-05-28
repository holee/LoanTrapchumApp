using Applications.Extensions;
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
        IEnumerable<VendorVm> Vendors = [];
        int pageIndex = 1;
        int pageSize = 10;
        int counts = 0;
        string sortField = "EntId";
        bool sortDescending = false;
        bool includeInactive = false;
        bool isLoading = false;
        string? vendorId = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetVendors();
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.Message, "Exception");
            }
            await base.OnInitializedAsync();

        }


        protected async Task OnSearch(string filter)
        {
            q = filter;
            await GetVendors(filter);
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
        async Task ShowEdit(string? vendorId) 
        {
            //NavigationManager.NavigateTo($"/App/Vendors/{id}/Edit");

            ContextMenuService.Close();

            if (vendorId.IsNotNullOrEmpty())
            {
                try
                {
                    IsEnableEdit = true;
                    selectedIndex = 2;
                    this.vendorId = vendorId;
                    await InvokeAsync(StateHasChanged);
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, ex.Message);
                }
            }


        }

        void SelectedId(string? id)
        {
            vendorId = id;
        }

        protected async Task DisplayDetail(string? vendorId)
        {
            ContextMenuService.Close();

            if (vendorId.IsNotNullOrEmpty())
            {
                try
                {
                    IsEnableDetail = true;
                    selectedIndex = 3;
                    this.vendorId = vendorId;
                    await InvokeAsync(StateHasChanged);
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
                }
            }
        }
        async Task OnDelete(string? id)
        {
            ContextMenuService.Close();
            var confirm = (bool)await DialogService.Confirm("Other Name deleted will not be able to recover, would you like to continue?", "Delete Other Name",
        new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            if (!confirm)
            {
                return;
            }
            var result = await _otherNameService!.DeleteAsync(id);
            if (result.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Delete Other Name", "Other Name was deleted successful!");
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

        async Task Closing(bool isEditTabl)
        {
            selectedIndex = 0;
            IsEnableEdit = isEditTabl;
            await GetVendors(q);
            StateHasChanged();
        }

        void OnEdit(string vendor_id)
        {
            selectedIndex = 2;
            IsEnableEdit = true;
            vendorId = vendor_id;
            StateHasChanged();
        }


        async Task CloseTabs(int e)
        {
            if (selectedIndex == 1)
            {
                IsEnableNew = false;
                selectedIndex = 0;
                await GetVendors(q);
            }
            else if (selectedIndex == 2)
            {
                IsEnableEdit = false;
                selectedIndex = 0;
                await GetVendors(q);
            }
            else if (selectedIndex == 3)
            {
                IsEnableDetail = false;
                selectedIndex = 0;

            }
            StateHasChanged();
        }

        protected async Task DeactiveOrActive(bool arg, VendorVm e)
        {
            //var entVendor = new EntVendorDtoChangeStatus
            //{
            //    VendorId = e.VendorId,
            //    EditSeq = e.EditSeq,
            //    IsActive = arg
            //};
            //var response = await _vendorService.ChangeVendorStatus(entVendor);
            //if (response.IsSuccess)
            //{
            //    NotificationService.Notify(NotificationSeverity.Success, "", response.Data);
            //    await GetVendors(q);
            //}
            //else
            //{
            //    await IncludeInActiveChanged(false);
            //    NotificationService.Notify(NotificationSeverity.Warning, "", response.ErrMessage);
            //}

            await Task.CompletedTask;
        }
    }
}