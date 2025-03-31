using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Addresses;
using Applications.Services.Lists;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
{
    public partial class Guarantors
    {
        private readonly IGuarantorServiceImpl _gService;
        public Guarantors(IGuarantorServiceImpl gService)
        {
            _gService = gService;
        }

        bool includeInactive = false;
        string? filterText = "";
        int pageIndex = 1;
        int pageSize = 10;
        string sortColumn = "Name";
        bool sortDescending = false;
        int counts = 0;
        bool isLoading = false;
        bool ShowAddTab = false;
        bool ShowDetailTab = false;
        bool ShowEditTab = false;
        RadzenTabs tabs = default!;
        int selectedTabIndex = 0;
        Guid? SelectedGuarantorId = Guid.Empty;
        IEnumerable<VGuarantor> guarantors = [];
        IEnumerable<ProvinceDto>? provinceList = [];
        IEnumerable<CommuneDto>? communeList = [];
        IEnumerable<DistrictDto>? districtList = [];
        IEnumerable<VillageDto>? villageList = [];
        EntityNameDtoCreate model = new EntityNameDtoCreate();


        void ChangedPageSize(int page)
        {
            pageSize = page;
        }

        async void SearchTextChanged()
        {
            await GetGuarantors();
        }

        async Task AddGuarantorTab()
        {
            if (!ShowAddTab)
            {
                ShowAddTab = true;
                provinceList = await _addressService!.GetsProvinces();
                model.Sex = "mr";
            }
            selectedTabIndex = 1;
            tabs.Reload();
        }

        void AddDetailGuarantorTab()
        {


            if (!ShowDetailTab)
            {
                ShowDetailTab = true;
            }
            selectedTabIndex = 2;
            tabs.Reload();
        }

        void Refresh() { }


        void SelectTabChanged(int index)
        {
            Console.WriteLine(index);
        }

        void CloseGuarantorTab()
        {
            if (ShowAddTab && selectedTabIndex == 1)
            {
                ShowAddTab = false;
                provinceList = [];
                model.Sex = "";
            }
            if (ShowDetailTab && selectedTabIndex == 2)
            {
                ShowDetailTab = false;
            }
            selectedTabIndex = 0;
        }
        //NavigationManager.NavigateTo($"/Customers/Guarantors/{id}/Details");
        void GetGuarantorDetails(Guid? id)
        {
            ContextMenuService.Close();
            if (!ShowDetailTab)
            {
                ShowDetailTab = true;
            }
            SelectedGuarantorId = id;
            selectedTabIndex = 2;
            tabs.Reload();
            Refresh();
        }

        async void GetEditGuarantor(Guid? id)
        {
            ContextMenuService.Close();

            var param = new Dictionary<string, object?> { { "NameId", id } };
            var options = new DialogOptions
            {
                Width = "80rem",
                Height = "30rem",
                Resizable = true,
                Draggable = true
            };

            var result = await DialogService.OpenAsync<GuarantorEdit>($"Edit Guarantor #{id}", param, options);

            if (((bool)result) == true)
            {
                await GetGuarantors();
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetGuarantors();
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

        async Task LoadGuarantors(LoadDataArgs e)
        {
            pageIndex = e.Skip!.Value;
            if (!string.IsNullOrEmpty(e.OrderBy))
            {
                sortColumn = e.OrderBy.Split(" ")[0];
                string dir = e.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }
            await GetGuarantors();

        }
        async Task ShowGuarantorModal()
        {
            var options = new DialogOptions
            {
                Width = "60rem",
                Height = "30rem",
                Draggable = true,
                AutoFocusFirstElement = true
            };
            var modalResult = await DialogService.OpenAsync<GuarantorAddModal>("Guarantor", options: options);

            if (bool.TryParse(modalResult, out bool result) && result == true)
            {
                await GetGuarantors();
            }

        }
        async Task IncludeInactiveCheckedChanged()
        {
            await GetGuarantors();
        }


        protected async Task OnSaveChanges()
        {
            model.EntType = "Other Name";

            var rsp = await _gService.CreateAsync(model);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Success, rsp.Data);
                ShowAddTab = false;
                selectedTabIndex = 0;
            }
            else
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);
            }
        }



        protected async Task DeleteGuarantorAsync(Guid? id)
        {

            if (id.IsNotNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Warning, "there is no any data selected.");
                return;
            }

            var confirm = await DialogService.Confirm($"do you want to delete guarantor with #{id}",
                                                "Delete", new ConfirmOptions
                                                {
                                                    OkButtonText = "Ok",
                                                    CancelButtonText = "Cancel"
                                                });
            if (confirm == false) return;

            var rsp = await _gService.DeleteAsync(id);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Success, rsp.Data);
                ShowAddTab = false;
                selectedTabIndex = 0;
                await GetGuarantors();
                tabs.Reload();
            }
            else
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);
            }
        }

        async void GetDistrictByProvince(object e)
        {
            if (e is not null)
            {
                model.ProInKhmer = ((ProvinceDto)e).ProInKhmer;
                var id = ((ProvinceDto)e).ProvinceId;
                districtList = await _addressService!.GetDistrictsByProvinceId(id);
            }
            StateHasChanged();
        }
        async void GeCommunesByDistrict(object e)
        {
            if (e is not null)
            {
                model.DisInKhmer = ((DistrictDto)e).DisInKhmer;
                var id = ((DistrictDto)e).DistrictId;
                communeList = await _addressService!.GetCommunesByDistrictId(id);
            }
            StateHasChanged();
        }
        void Cancel()
        {
            DialogService.Close(false);
        }
        async void GetVillagesByCommune(object e)
        {
            if (e is not null)
            {
                model.ComInKhmer = ((CommuneDto)e).ComInKhmer;
                var id = ((CommuneDto)e).CommuneId;
                villageList = await _addressService!.GetVillagesByCommuneId(id);
            }
            StateHasChanged();
        }
        void GetVillagesName(object e)
        {
            if (e is not null)
            {
                model.VilInKhmer = ((VillageDto)e).VilInKhmer;
            }
            StateHasChanged();
        }
        void SetFirstName()
        {
            model.FirstName = model.Name;
        }
    }
}