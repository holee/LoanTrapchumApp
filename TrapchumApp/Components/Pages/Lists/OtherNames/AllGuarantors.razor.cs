using Applications.Extensions;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
{
    public partial class AllGuarantors
    {

        private readonly IGuarantorServiceImpl _gService;
        public AllGuarantors(IGuarantorServiceImpl gService)
        {
            _gService = gService;
        }

        bool includeInactive = true;
        string? filterText = "";
        int pageIndex = 0;
        int pageSize = 10;
        string sortColumn = "FullName";
        bool sortDescending = false;
        int counts = 0;
        bool isLoading = false;
        bool ShowAddTab = false;
        RadzenTabs tabs = default!;
        int selectedTabIndex = 0;
        string? SelectedGuarantorId = string.Empty;
        IEnumerable<VGuarantor> guarantors = [];
        EntityNamesCreate modelCreate = new EntityNamesCreate();
        EntityNamesCreate modelEdit = new EntityNamesCreate();
        [Parameter] public EventCallback<int> OpenAddTab { get; set; }
        [Parameter] public EventCallback<int> OpenDetailTab { get; set; }
        [Parameter] public EventCallback<int> OpenEditTab { get; set; }
        [Parameter] public EventCallback<string> SetGuarantorId { get; set; }
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

        protected override async Task OnInitializedAsync()
        {
            await GetGuarantors();

            await base.OnInitializedAsync();
        }

        async Task IncludeInactiveCheckedChanged()
        {
            await GetGuarantors();
        }

        async Task ChangedPageSize(int page)
        {
            pageSize = page;
            await GetGuarantors();
        }

        async void SearchTextChanged()
        {
            await GetGuarantors();
        }

        async Task AddAddTab()
        {
            await OpenAddTab.InvokeAsync(1);
        }
        async Task Reload()
        {
            await GetGuarantors();
        }

        async Task GetGuarantors()
        {
            isLoading = true;
            var param = new QueryParam(pageIndex, pageSize, sortColumn, includeInactive, sortDescending,filterText);
            var guarantorResp = await _gService.GetGuarantorsAsync(param);
            if (guarantorResp.IsSuccess)
            {
                guarantors = guarantorResp.Data.Items;

                counts = guarantorResp.Data.Count;
            }
            isLoading = false;

            StateHasChanged();
        }

        async Task GetGuarantorDetails(string? id)
        {
            ContextMenuService.Close();
            await SetGuarantorId.InvokeAsync(id);
            await OpenDetailTab.InvokeAsync(3);
        }

        async Task GetEditGuarantor(string? id)
        {
            ContextMenuService.Close();
            await SetGuarantorId.InvokeAsync(id);
            await OpenEditTab.InvokeAsync(2);
            StateHasChanged();

        }

        protected async Task DeleteGuarantorAsync(string? id)
        {

            if (id.IsNullOrEmpty())
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
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
                ShowAddTab = false;
                selectedTabIndex = 0;
                await GetGuarantors();
                tabs.Reload();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }



    }
}