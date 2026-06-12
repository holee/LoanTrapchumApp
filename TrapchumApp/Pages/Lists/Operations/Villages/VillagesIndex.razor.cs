using Applications.Interfaces.Addresses;
using Applications.Responses.ViewModels.Addresses;

namespace TrapchumApp.Pages.Lists.Operations.Villages
{
    public partial class VillagesIndex(IAddressService villageServer, ILogger<VillagesIndex> logger)
    {
        protected string? searchText = string.Empty;
        protected bool includeInactive = false;
        int pageIndex = 0;
        int pageSize = 10;
        int counts = 0;
        bool sortDescending = false;
        string sortField = "";
        protected List<VVillages> vVillageList = [];


        protected override async Task OnInitializedAsync()
        {
            await GetVillages();
            await base.OnInitializedAsync();
        }
        protected async Task GetVillages(string? text = null)
        {
            try
            {

                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, text);
                var results = await villageServer.GetAllVillagesPagerAsync(param);
                if (results.Count > 0)
                {
                    
                    counts = results.Count;
                    vVillageList = results.Items;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading villages: {Message}", ex.Message);
            }
        }
        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "VillageId";
            bool sortDescending = false;
            pageIndex = args.Skip!.Value;


            if (args.OrderBy.Any())
            {
                sortField = args.Sorts.First().Property;

                sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
            }

            var filterColumns = args.Filters?.FirstOrDefault(f =>
                                                                f.Property == nameof(VVillages.VilInKhmer) ||
                                                                f.Property == nameof(VVillages.VillDescription) ||
                                                                f.Property == nameof(VVillages.ComInKhmer) ||
                                                                f.Property == nameof(VVillages.ComDescription))?.FilterValue?.ToString();

            var filterString = filterColumns ?? string.Empty;

            await LoadVillagePageList(pageIndex, sortField, sortDescending,filterString);
        }

        async Task LoadVillagePageList(int pageIndex = 0, string sortField = "VillageId", bool sortDescending = false,string? filter=null)
        {
            try
            {

                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending,  filter);

                var res = await villageServer.GetAllVillagesPagerAsync(param);

                vVillageList = res.Items;
                counts = res.Count;
                StateHasChanged();
            }
            catch
            {
                throw new Exception("Error loading villages. Please try again later.");
            }
        }
        protected async Task TextChanged(string? searchtext = null)
        {
            searchText = searchtext ?? string.Empty;
            await GetVillages(searchText);
        }

        protected async Task ShowCreate()
        {
            var resultModal = await DialogService!.OpenAsync<VillageCreatePage>("Village Create",
               options: new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,
               });
            if (resultModal != null)
            {
                await GetVillages();

                await InvokeAsync(StateHasChanged);
            }
        }
        protected async Task Edit(int Id)
        {
            var resultModal = await DialogService!.OpenAsync<VillageUpdatePage>("Edit Village",
               new Dictionary<string, object> { { "Id", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {
                await GetVillages();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }
        protected async Task Details(int Id)
        {
            var resultModal = await DialogService!.OpenAsync<VillageDetailsPage>("Details Village",
               new Dictionary<string, object> { { "Id", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {

            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task Delete(int id)
        {
            try
            {
                var confirm = await DialogService.Confirm("Do you want to delete village record?", "Delete", new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "OK",
                });
                if (confirm == true)
                {

                    var result = await villageServer.DeleteVillageAsync(id);

                    if (!result.IsError)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, "Village one record deleted successful.");
                        await GetVillages(searchText);
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Success, result.FirstError.Description);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Error deleting village: {ex.Message}");
            }
           

        }
    }
}