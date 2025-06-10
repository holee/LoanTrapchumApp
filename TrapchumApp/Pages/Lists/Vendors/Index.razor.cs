using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Vendors;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class Index
    {
        readonly IVendorServiceImpl _vendorService;

        readonly IOtherNameService _otherNameService;
        public Index(IVendorServiceImpl vendorService, IOtherNameService otherNameService)
        {
            _vendorService = vendorService;

            _otherNameService = otherNameService;
        }

        [Parameter] public int selectedIndex { get; set; } = 0;
        RadzenTabs? tabs;
        bool IsEnableEdit = false;
        bool IsEnableNew = false;
        bool IsEnableDetail = false;
        string? filter;
        long NameId { get; set; }
        IEnumerable<ProvinceDto>? ProvinceList = [];
        EntNameDtoUpdate? model = new();
        EntNameDto? OtherNames = new();
        List<VendorVm> Vendors = [];
        int pageIndex = 0;
        int pageSize = 15;
        int counts = 0;
        string sortField = "VendorId";
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


        protected async Task SearchChanged(string filter)
        {
            this.filter = filter;
            await GetVendors(filter);
        }

        async Task PageSizeChanged(int page)
        {
            pageSize = page;
            pageIndex = 0;
            await GetVendors(filter);
        }

        async Task GetVendors(string? filterText = "")
        {

            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filterText);

            var resp = await _vendorService.GetVendorsAsync(param);

            counts = resp.Count;

            Vendors = resp.Items;

            isLoading = false;

            StateHasChanged();
        }

        async Task LoadData(LoadDataArgs e)
        {

            pageIndex = e.Skip!.Value;

            pageSize = e.Top ?? 15;

            (sortField, sortDescending) = e.PageInfo();

            var filterColumn = e.Filters?.FirstOrDefault(pr => pr.Property == nameof(VendorVm.AltName) ||
                                                           pr.Property == nameof(VendorVm.VendorId) ||
                                                           pr.Property == nameof(VendorVm.FirstName) ||
                                                           pr.Property == nameof(VendorVm.Salutation) ||
                                                           pr.Property == nameof(VendorVm.LastName) ||
                                                           pr.Property == nameof(VendorVm.Phone) ||
                                                           pr.Property == nameof(VendorVm.EmailAddress))?.FilterValue.ToString() ?? string.Empty;
            filter = filterColumn ?? string.Empty;

            await GetVendors(filter);

        }
        async Task ShowEdit(string? vendorId)
        {

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
                await GetVendors(filter);
            }

        }
        protected async Task IncludeInActiveChanged(bool e)
        {
            includeInactive = e;
            await GetVendors(filter);
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
            await GetVendors(filter);
            StateHasChanged();
        }

        void OnEdit(string vendor_id)
        {
            selectedIndex = 2;
            IsEnableEdit = true;
            vendorId = vendor_id;
            StateHasChanged();
        }

        async Task CloseTabs(int index)
        {
            if (index == 1)
            {
                IsEnableNew = false;
            }
            if (index == 2)
            {
                IsEnableEdit = false;
            }
            if (index == 3)
            {
                IsEnableDetail = false;
            }

            selectedIndex = 0;

            await GetVendors(filter);

            StateHasChanged();
        }

        protected async Task DeactiveOrActive(bool arg, string vendorId)
        {

            var confirm = await DialogService.Confirm($"Do you want to change vendor ID #{vendorId} status?", "Confirm",
                options: new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "Yes",
                });

            if (confirm == false) return;

            var rsp = await _vendorService.ChangeVendorStatus(vendorId, arg);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

                await GetVendors(filter);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

        }
    }
}