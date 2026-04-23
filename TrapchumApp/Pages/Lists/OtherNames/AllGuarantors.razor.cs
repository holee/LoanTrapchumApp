using Applications.Extensions;
using Applications.Features.Guarantor.Dtos;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Radzen.Blazor;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.OtherNames
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

        int pageSize = 15;

        string sortColumn = "GuarantorName";

        bool sortDescending = false;

        int counts = 0;

        bool isLoading = false;

        bool ShowAddTab = false;

        RadzenTabs tabs = default!;

        int selectedTabIndex = 0;

        string? SelectedGuarantorId = string.Empty;

        List<VGuarantor> guarantors = [];
        [Parameter] public EventCallback<string> ShowDetails { get; set; } 
        [Parameter] public EventCallback<string> ShowEdit { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        async Task LoadGuarantors(LoadDataArgs e)
        {
            pageIndex = e.Skip ?? 0;

            pageSize = e.Top ?? 15;

            (sortColumn, sortDescending) = e.PageInfo();

            var filterDescriptor = e.Filters?.FirstOrDefault();

            var filterProperty = filterDescriptor?.Property;

            var filterValue = filterDescriptor?.FilterValue?.ToString();

            filterText = filterValue ?? string.Empty;

            await GetGuarantors(filterText);

        }

        protected override async Task OnInitializedAsync()
        {
            await GetGuarantors();

            await base.OnInitializedAsync();
        }

        async Task IncludeInactiveCheckedChanged(bool arg)
        {
            includeInactive = arg;

            await GetGuarantors(filterText);
        }

        async Task ChangedPageSize(int page)
        {
            pageSize = page;
            pageIndex = 0;
            await GetGuarantors();
        }

        async void SearchTextChanged(string filter)
        {

            filterText = filter;

            await GetGuarantors(filter);
        }

        async Task GetGuarantors(string? filter = null)
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, includeInactive, sortDescending, filter);

            var rsp = await _gService.GetGuarantorsAsync(param);

            guarantors = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            StateHasChanged();
        }

        async Task GetGuarantorDetails(string? id)
        {
            ContextMenuService.Close();

            if (!string.IsNullOrEmpty(id))
            {
                await ShowDetails.InvokeAsync(id);
            }
        }

        async Task GetEditGuarantor(string? id)
        {
            ContextMenuService.Close();

            if (!string.IsNullOrEmpty(id))
            {
                await ShowEdit.InvokeAsync(id);
            }
           
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

            if (!rsp.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Success, "A guarantor was removed");

                ShowAddTab = false;

                selectedTabIndex = 0;

                await GetGuarantors();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
            }
        }



    }
}