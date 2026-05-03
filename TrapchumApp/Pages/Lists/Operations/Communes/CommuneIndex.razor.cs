using Applications.Interfaces.Addresses;
using Applications.Responses.ViewModels.Addresses;
using TrapchumApp.Pages.Lists.Operations.Villages;

namespace TrapchumApp.Pages.Lists.Operations.Communes
{
    public partial class CommuneIndex
    {
        private readonly IAddressService communeService;
        public CommuneIndex(IAddressService communeService)
        {
            this.communeService = communeService;
        }
        protected string? searchText = string.Empty;
        protected bool includeInactive = false;
        int pageIndex = 0;
        int pageSize = 10;
        int counts = 0;
        bool sortDescending = false;
        string sortField = "";
        protected List<VCommunes> vCommunelistt = [];


        protected override async Task OnInitializedAsync()
        {
            await GetCommunes();
            await base.OnInitializedAsync();
        }
        protected async Task GetCommunes(string? text = null)
        {
            var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, text);
            var results = await communeService.GetAllCommunesPagerAsync(param);
            counts = results.Count;
            vCommunelistt = results.Items;
        }
        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "CommuneId";
            bool sortDescending = false;
            pageIndex = args.Skip!.Value;
            if (args.OrderBy.Any())
            {
                sortField = args.Sorts.First().Property;

                sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
            }

            var filterColumns = args.Filters?.FirstOrDefault(f => 
                                                                f.Property == nameof(VCommunes.DisDescription) ||
                                                                f.Property == nameof(VCommunes.DisInKhmer) ||
                                                                f.Property == nameof(VCommunes.ComInKhmer) ||
                                                                f.Property == nameof(VCommunes.ComDescription))?.FilterValue?.ToString();

            var filterString = filterColumns ?? string.Empty;

            await LoadDistrictPageList(pageIndex, sortField, sortDescending,filterString);
        }

        async Task LoadDistrictPageList(int pageIndex = 0, string sortField = "CommuneId", bool sortDescending = false,string? filter=null)
        {
            var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);
            var res = await communeService.GetAllCommunesPagerAsync(param);
            vCommunelistt = res.Items;
            counts = res.Count;
            StateHasChanged();
        }
        protected async Task TextChanged(string? searchtext = null)
        {
            searchText = searchtext ?? string.Empty;
            await GetCommunes(searchText);
        }

        protected async Task ShowCreate()
        {
            var resultModal = await DialogService!.OpenAsync<CommuneCreatePage>("Commune Create",
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
            var resultModal = await DialogService!.OpenAsync<CommuneUpdatePage>("Edit Commune",
               new Dictionary<string, object> { { "Id", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {
                await GetCommunes();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }

        protected async Task AddVillage(int? Id) 
        {
            var resultModal = await DialogService!.OpenAsync<VillageCreatePage>("Village Create",
               new Dictionary<string, object?> { { "Id", Id } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {
                await GetCommunes();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }

        protected async Task Delete(int id)
        {
            try
            {
                var confirm = await DialogService.Confirm("Do you want to delete commune record?", "Delete", new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "OK",
                });
                if (confirm == true)
                {
                    var used = vCommunelistt.Where(x => x.CommuneId == id).FirstOrDefault();
                    if (used!.Used == true)
                    {
                        NotificationService.Notify(NotificationSeverity.Warning, "This commune's record in used with other tables, cannot delete it.");
                        return;
                    }
                    var result = await communeService.DeleteCommuneAsync(id);

                    if (!result.IsError)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, "Commune one record deleted successful.");
                        await GetCommunes(searchText);
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, result.FirstError.Description);
                    }
                }
            }
            catch(Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
                return;
            }
            

        }
    }
}