using Applications.Interfaces.Addresses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Addresses;
using System.Threading.Tasks;
using TrapchumApp.Pages.Lists.Operations.Districts;

namespace TrapchumApp.Pages.Lists.Operations.Provinces
{
    public partial class ProvinceIndex
    {
        private readonly IProvinceService _proService;
        private readonly IDistrictService districtService;

        public ProvinceIndex(IProvinceService proService, IDistrictService districtService)
        {
            _proService = proService;
            this.districtService = districtService;
        }
        protected string? searchText = string.Empty;

        protected bool includeInactive = false;

        int pageIndex = 0;

        int pageSize = 10;

        int counts = 0;

        bool sortDescending = false;

        string sortField = "";

        protected List<ProvinceViewModel> ProvincesList = [];
        protected List<District> districtList = [];
        protected override async Task OnInitializedAsync()
        {
            await GetProvinces();
            await base.OnInitializedAsync();
        }
        async Task PageSizeChanged(int page)
        {
            pageSize = page;
            pageIndex = 0; // Reset to first page when page size changes
            await GetProvinces();
        }
        private async Task GetProvinces(string? filter = null)
        {
            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filter);
                var resultResponse = await _proService.GetAllAsync(param);
                counts = resultResponse.Count;
                ProvincesList = resultResponse.Items ?? [];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading provinces: {ex.Message}");
            }

        }
        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "ProvinceId";

            bool sortDescending = false;

            pageIndex = args.Skip!.Value;

            if (args.OrderBy.Any())
            {
                sortField = args.Sorts.First().Property;

                sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
            }

            var filterColumns = args.Filters?.FirstOrDefault(f => f.Property == nameof(ProvinceViewModel.ProvinceCode) ||
                                                                f.Property == nameof(ProvinceViewModel.ProDescription) ||
                                                                f.Property == nameof(ProvinceViewModel.ProInKhmer))?.FilterValue?.ToString();

            var filterString = filterColumns ?? string.Empty;

            await LoadProvincePageList(pageIndex, sortField, sortDescending, filterString);
        }

        async Task LoadProvincePageList(int pageIndex = 0, string sortField = "ProvinceId", bool sortDescending = false, string? filter = null)
        {
            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);

                var res = await _proService.GetAllAsync(param);

                ProvincesList = res.Items;

                counts = res.Count;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading provinces: {ex.Message}");
            }

        }
        protected async Task TextChanged(string? searchtext = null)
        {
            searchText = searchtext ?? string.Empty;
            await GetProvinces(searchText);
        }

        protected async Task ShowCreate()
        {
            var resultModal = await DialogService!.OpenAsync<ProvinceCreatePage>("Province Create",
               options: new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,
               });
            if (resultModal != null)
            {
                await InvokeAsync(StateHasChanged);
            }
        }
        protected async Task Edit(int Id)
        {
            var resultModal = await DialogService!.OpenAsync<ProvinceUpdatePage>("Edit Province",
               new Dictionary<string, object> { { "Id", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {
                await GetProvinces();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }


        protected async Task AddDistrict(int? Id)
        {
            await DialogService!.OpenAsync<DistrictCreatePage>("District Create",
               new Dictionary<string, object?> { { "ProvinceCode", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = false,

               });

        }


        protected async Task Details(int Id)
        {

        }
        protected async Task Delete(int proId)
        {
            try
            {
                var confirm = await DialogService.Confirm("Do you want to delete?", "Delete", new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "OK",
                });
                if (confirm == true)
                {
                    var datas = await districtService.GetAllAsync();
                    districtList = datas.Data.Where(x => x.ProvinceRefId == proId).ToList();
                    if (districtList.Count() > 0)
                    {
                        NotificationService.Notify(NotificationSeverity.Warning, "This province's record in used with other tables, cannot delete it.");
                        return;
                    }
                    var result = await _proService.DeleteAsync(proId);

                    if (result.IsSuccess)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, "Province one record deleted successful.");
                        await GetProvinces(searchText);
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Warning, result.ErrMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
                return;
            }


        }
    }
}