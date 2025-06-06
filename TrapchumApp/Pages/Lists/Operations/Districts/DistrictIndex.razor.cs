
using Applications.Interfaces.Addresses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Radzen;
using TrapchumApp.Pages.Lists.Operations.Communes;
using TrapchumApp.Pages.Lists.Operations.Provinces;

namespace TrapchumApp.Pages.Lists.Operations.Districts
{
    public partial class DistrictIndex
    {
        private readonly IDistrictService districtService;

        public DistrictIndex(IDistrictService districtService) {
            this.districtService = districtService;
        }
        protected string? searchText = string.Empty;
        protected bool includeInactive = false;
        int pageIndex = 0;
        int pageSize = 10;
        int counts = 0;
        bool sortDescending = false;
        string sortField = "";
        protected List<VDistricts> VDistrictList = [];
        protected override async Task OnInitializedAsync()
        {
            await GetDistricts();
            await base.OnInitializedAsync();
        }
         protected async Task GetDistricts(string? text = null)
        {
            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, text);
                var results = await districtService.GetAllPagerAsync(param);
                if (results.Count > 0)
                {
                    counts=results.Count;
                    VDistrictList = results.Items;
                }

            }
            catch (Exception ex)
            {

            }
        }
        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "DistrictId";
            bool sortDescending = false;
            pageIndex = args.Skip!.Value;
            if (args.OrderBy.Any())
            {
                sortField = args.Sorts.First().Property;

                sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
            }

            var filterColumns = args.Filters?.FirstOrDefault(f => 
                                                                f.Property == nameof(VDistricts.DisDescription) ||
                                                                f.Property == nameof(VDistricts.DisInKhmer) ||
                                                                f.Property == nameof(VDistricts.ProInKhmer)||
                                                                f.Property == nameof(VDistricts.ProDescription))?.FilterValue?.ToString();

            var filterString = filterColumns ?? string.Empty;

            await LoadDistrictPageList(pageIndex, sortField, sortDescending,filterString);
        }

        async Task LoadDistrictPageList(int pageIndex = 0, string sortField = "DistrictId", bool sortDescending = false,string? filter=null)
        {
            try
            {

                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);

                var res = await districtService.GetAllPagerAsync(param);

                VDistrictList = res.Items;
                counts = res.Count;
                StateHasChanged();
            }
            catch
            {

            }
        }
        protected async Task TextChanged(string? searchtext = null)
        {
            searchText = searchtext ?? string.Empty;
            await GetDistricts(searchText);
        }

        protected async Task ShowCreate()
        {
            var resultModal = await DialogService!.OpenAsync<DistrictCreatePage>("District Create",
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
            var resultModal = await DialogService!.OpenAsync<DistrictUpdatePage>("Edit District",
               new Dictionary<string, object> { { "Id", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {
                await GetDistricts();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }
        protected async Task AddCommune(int Id)
        {
            var resultModal = await DialogService!.OpenAsync<CommuneCreatePage>("Commune Create",
               new Dictionary<string, object> { { "Id", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {
                await GetDistricts();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }
        protected async Task Details(int Id)
        {

        }
        protected async Task Delete(int proId)
        {
            var confirm = await DialogService.Confirm("Do you want to delete district record?", "Delete", new ConfirmOptions
            {
                CancelButtonText = "Cancel",
                OkButtonText = "OK",
            });
            if (confirm == true)
            {
                var used= VDistrictList.Where(x=>x.DistrictId == proId).FirstOrDefault();
                if (used.Used == true)
                {
                    NotificationService.Notify(NotificationSeverity.Warning, "This district's record in used with other tables, cannot delete it.");
                    return;
                }
                var result = await districtService.DeleteAsync(proId);

                if (result.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "District one record deleted successful.");
                    await GetDistricts(searchText);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Success, result.ErrMessage);
                }
            }

        }
    }
}