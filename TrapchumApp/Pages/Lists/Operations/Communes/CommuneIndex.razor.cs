using Applications.Interfaces.Addresses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Infrastructures.Implements;
using TrapchumApp.Pages.Lists.Operations.Districts;

namespace TrapchumApp.Pages.Lists.Operations.Communes
{
    public partial class CommuneIndex
    {
        private readonly ICommuneService communeService;
        public CommuneIndex(ICommuneService communeService) {
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
            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, text);
                var results = await communeService.GetAllPagerAsync(param);
                if (results.Count > 0)
                {
                    counts = results.Count;
                    vCommunelistt = results.Items;
                }

            }
            catch (Exception ex)
            {

            }
        }
        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "CommuneId";
            bool sortDescending = false;
            pageIndex = args.Skip!.Value;
            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortField = args.OrderBy.Split(" ")[0];
                string dir = args.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }

            await LoadDistrictPageList(pageIndex, sortField, sortDescending);
        }

        async Task LoadDistrictPageList(int pageIndex = 0, string sortField = "CommuneId", bool sortDescending = false)
        {
            try
            {

                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, searchText);

                var res = await communeService.GetAllPagerAsync(param);

                vCommunelistt = res.Items;
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
        protected async Task Details(int Id)
        {

        }
        protected async Task Delete(int id)
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
                var result = await communeService.DeleteAsync(id);

                if (result.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Commune one record deleted successful.");
                    await GetCommunes(searchText);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Success, result.ErrMessage);
                }
            }

        }
    }
}