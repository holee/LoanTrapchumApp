using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
{
    public partial class GuarantorsList
    {

        private readonly IGuarantorServiceImpl _gService;

        public GuarantorsList(IGuarantorServiceImpl gService)
        {
            _gService = gService;
        }

        [Parameter]
        public IList<VGuarantor> SelectedGuarantors { get; set; } = [];
        string? filterText = "";
        int pageSize = 10;
        string sortColumn = "Name";
        int counts = 0;
        bool isLoading = false;
        bool allowRowSelectOnRowClick = true;
        IEnumerable<VGuarantor> Guarantors = [];
        IEnumerable<VGuarantor> newGuarantors = [];
        RadzenDataGrid<VGuarantor> grid = default!;

        void SearchTextChanged(ChangeEventArgs e)
        {
            filterText = e.Value!.ToString() ?? "";
            newGuarantors = Guarantors.Where(x => x.GuarantorName!.Contains(filterText, StringComparison.InvariantCultureIgnoreCase) ||
                                                  x.Phone!.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)) ?? [];
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetGuarantors();
        }
        async Task GetGuarantors()
        {
            isLoading = true;
            var guarantorResp = await _gService.GetGuarantorsAsync(null, true, filterText);
            if (guarantorResp.IsSuccess)
            {
                Guarantors = guarantorResp.Data!;
                counts = guarantorResp.Data!.Count();
                newGuarantors = Guarantors;
            }
            isLoading = false;

            StateHasChanged();
        }


        bool GuarantorsChecked(VGuarantor g)
        {
            return SelectedGuarantors.Any() && SelectedGuarantors.Contains(g);

        }

        async void GuarantorCheckedChanged(bool args, VGuarantor g)
        {
            if (args && !allowRowSelectOnRowClick)
            {
                await grid.SelectRow(g);
            }
        }


        void AcceptSelectedGuarantors()
        {
            if (!SelectedGuarantors.Any())
            {
                NotificationService.Notify(NotificationSeverity.Info, "There is no any gaurator selected.");
                return;
            }
            DialogService.Close(SelectedGuarantors);
        }

        void Cancel()
        {
            DialogService.Close(SelectedGuarantors);
        }


        // async Task LoadGuarantors(LoadDataArgs e)
        // {
        //     pageIndex = e.Skip!.Value;
        //     if (!string.IsNullOrEmpty(e.OrderBy))
        //     {
        //         sortColumn = e.OrderBy.Split(" ")[0];
        //         string dir = e.OrderBy.Split(" ")[1];
        //         sortDescending = dir.Contains("asc") ? false : true;
        //     }
        //     await GetGuarantors();

        // }

        // async Task IncludeInactiveCheckedChanged()
        // {
        //     await GetGuarantors();
        // }

        // async Task ShowGuarantorModal()
        // {
        //     var options = new DialogOptions
        //         {
        //             Width = "60rem",
        //             Height = "30rem",
        //             Draggable = true,
        //             AutoFocusFirstElement = true
        //         };
        //     var modalResult = await DialogService.OpenAsync<GuarantorAddModal>("Guarantor", options: options);

        //     if (bool.TryParse(modalResult, out bool result) && result == true)
        //     {
        //         await GetGuarantors();
        //     }

        // }

        // protected async Task OnSaveChanges()
        // {
        //     model.EntType = "Other Name";
        //     var resultResponse = await _gService.CreateGaurantorAsync(model);

        //     if (resultResponse.IsSuccess)
        //     {
        //         NotificationService.Notify(Radzen.NotificationSeverity.Success, resultResponse.Data!.message);
        //         ShowAddTab = false;
        //         selectedTabIndex = 0;
        //         //tabs.Reload();
        //     }
        //     else
        //     {
        //         var error = resultResponse.Errors!.FirstOrDefault();
        //         NotificationService.Notify(Radzen.NotificationSeverity.Error, error);
        //     }
        // }



        // protected async Task DeleteGuarantorAsync(long id)
        // {

        //     if (id <= 0)
        //     {
        //         NotificationService.Notify(NotificationSeverity.Warning, "there is no any data selected.");
        //         return;
        //     }

        //     var confirm = await DialogService.Confirm($"do you want to delete guarantor with #{id}",
        //                                         "Delete", new ConfirmOptions
        //                                             {
        //                                                 OkButtonText = "Ok",
        //                                                 CancelButtonText = "Cancel"
        //                                             });
        //     if (confirm == false) return;

        //     var resultResponse = await _gService.DeleteAsync(id);

        //     if (resultResponse.IsSuccess)
        //     {
        //         NotificationService.Notify(Radzen.NotificationSeverity.Success, resultResponse.Data!.message);
        //         ShowAddTab = false;
        //         selectedTabIndex = 0;
        //         await GetGuarantors();
        //         tabs.Reload();
        //     }
        //     else
        //     {
        //         var error = resultResponse.Errors!.FirstOrDefault();
        //         NotificationService.Notify(Radzen.NotificationSeverity.Error, error);
        //     }
        // }

        // async void GetDistrictByProvince(object e)
        // {
        //     if (e is not null)
        //     {
        //         model.ProInKhmer = ((ProvinceDto)e).ProInKhmer;
        //         var id = ((ProvinceDto)e).ProvinceID;
        //         districtList = await _addressService!.GetDistrictByProvinceAsync(id);
        //     }
        //     StateHasChanged();
        // }
        // async void GeCommunesByDistrict(object e)
        // {
        //     if (e is not null)
        //     {
        //         model.DisInKhmer = ((DistrictDto)e).DisInKhmer;
        //         var id = ((DistrictDto)e).DistrictId;
        //         communeList = await _addressService!.GetCommuneByDistricteAsync(id);
        //     }
        //     StateHasChanged();
        // }



        // async void GetVillagesByCommune(object e)
        // {
        //     if (e is not null)
        //     {
        //         model.ComInKhmer = ((CommuneDto)e).ComInKhmer;
        //         var id = ((CommuneDto)e).CommuneId;
        //         villageList = await _addressService!.GetVillageByCommuneAsync(id);
        //     }
        //     StateHasChanged();
        // }
        // void GetVillagesName(object e)
        // {
        //     if (e is not null)
        //     {
        //         model.VilInKhmer = ((VillageDto)e).VilInKhmer;
        //     }
        //     StateHasChanged();
        // }
        // void SetFirstName()
        // {
        //     model.FirstName = model.Name;
        // }

    }
}