using Applications.Interfaces.Addresses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;

namespace TrapchumApp.Pages.Lists.Operations.Provinces
{
    public partial class ProvinceIndex
    {
        private readonly IProvinceService _proService;

        public ProvinceIndex(IProvinceService proService)
        {
            _proService = proService;
        }
        protected string? searchText = string.Empty;

        protected bool includeInactive = false;

        int pageIndex = 0;

        int pageSize = 10;

        int counts= 0;

        bool sortDescending = false;

        string sortField = "";

        protected List<ProvinceViewModel> ProvincesList= [];
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetProvinces();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            await base.OnInitializedAsync();
        }
        void PageSizeChanged(int page)
        {
            pageSize = page;
        }
        private async Task GetProvinces(string? filter = null)
        {   
                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filter);
                var resultResponse = await _proService.GetAllAsync(param);
                counts = resultResponse.Count;
                ProvincesList = resultResponse.Items ?? [];
        }
        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "ProvinceId";

            bool sortDescending = false;

            int pageIndex = args.Skip!.Value;

            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortField = args.OrderBy.Split(" ")[0];

                string dir = args.OrderBy.Split(" ")[1];

                sortDescending = dir.Contains("asc") ? false : true;
            }

            await LoadProvincePageList(pageIndex, sortField, sortDescending);
        }

        async Task LoadProvincePageList(int pageIndex = 0, string sortField = "ProvinceId", bool sortDescending = false)
        {
                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, searchText);

                var res = await _proService.GetAllAsync(param);

                ProvincesList = res.Items;

                counts = res.Count;

                StateHasChanged();
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
                await OnInitializedAsync();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }
        protected async Task Details(int Id)
        {

        }
        protected async Task Delete(int proId)
        {
            var confirm = await DialogService.Confirm("Do you want to delete?", "Delete", new ConfirmOptions
            {
                CancelButtonText = "Cancel",
                OkButtonText = "OK",
            });
            if (confirm == true)
            {
                var result = await _proService.DeleteAsync(proId);

                if (result.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Province one record deleted successful.");
                    await GetProvinces(searchText);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Success, result.ErrMessage);
                }
            }

        }
    }
}