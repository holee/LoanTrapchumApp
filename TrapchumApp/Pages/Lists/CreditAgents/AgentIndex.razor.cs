using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Applications.Services.Lists;
using Radzen.Blazor;
using TrapchumApp.Pages.Identity.Users;
using TrapchumApp.Helpers.Extensions;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;

namespace TrapchumApp.Pages.Lists.CreditAgents;

public partial class AgentIndex
{
    [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;
    [Parameter] public int Index { get; set; } = 1;

    private readonly ICreditAgentServiceImpl _creditAgentService;

    public AgentIndex(ICreditAgentServiceImpl creditAgentService)
    {
        _creditAgentService = creditAgentService;
    }
    int BranchId;
    string? BranchName;
    int selectedBranchId = 0;
    bool isLoading = false;
    bool ShowAddedTab = false;
    RadzenTabs tabs = default!;
    protected bool includeInactive = false;
    protected IEnumerable<EntEmployeeDto> Employees { get; set; } = [];
    protected List<VCreditAgent>? CreditAgents { get; set; }

    protected string? _filter = "";

    List<VBranch> Branches = [];
    protected string? UserId { get; set; }

    int pageIndex = 0;

    int pageSize = 15;

    string sortColumn = "FirstName";

    bool sortDescending = false;

    int counts = 0;

    LoanCreditAgentDtoCreate model = new LoanCreditAgentDtoCreate();

    List<VEmployeeCreditAgent> empCreditAgents = [];

    int selectedTabIndex = 0;

    async Task ChangedPageSize(int page)
    {
        pageSize = page;

        pageIndex = 0;

        await GetCreditAgents(selectedBranchId, _filter);
    }




    async void SelectedTabChanged(int index)
    {
        selectedTabIndex = index;

        if (selectedTabIndex == 1)
        {
            await LoadPageList();
        }
    }

    async Task AddCreditAgentTab()
    {
        if (!ShowAddedTab)
        {
            ShowAddedTab = true;

            await LoadPageList();
        }
        selectedTabIndex = 1;
        //tabs.Reload();
    }
    void RemoveTab()
    {
        if (selectedTabIndex == 1 && ShowAddedTab)
        {
            ShowAddedTab = false;
        }
        selectedTabIndex = 0;
    }



    async Task LoadPageList(string? filter = null)
    {
        var rsp = await _creditAgentService.GetEmployeeCreateCreditAgents(filter);

        if (rsp.IsSuccess)
        {
            empCreditAgents = rsp.Data!.ToList();
        }
    }

    async void Reload()
    {
        await GetCreditAgents(BranchId, _filter);
    }


    async Task AddUserAccount(VCreditAgent c)
    {

        var options = new DialogOptions
        {
            Width = "620px",
            ContentCssClass = "rz-mt-3 rz-background-color-base-100"
        };

        var param = new Dictionary<string, object?>
        {
            {"EmployeeId",c.EmployeeRefId},
            {"BranchId",c.BranchId}
        };

        bool? dialogueResult = await DialogService.OpenAsync<CreditAgentUserCreate>("New User", param, options: options);

        if (dialogueResult is not null && dialogueResult == true)
        {

        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {

            var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0, string.Empty));

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess)
            {
                Branches = rsp.Items;

                Branches.Add(new VBranch
                {
                    BranchId = 0,
                    BranchName = "All"
                });

                Branches = Branches.OrderBy(x => x.BranchId).ToList();

                BranchId = Branches.First().BranchId;
            }
            await GetCreditAgents(BranchId, _filter);
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }

        await base.OnInitializedAsync();
    }

    protected async Task GetCreditAgents(int? branchId = 0, string? q = null)
    {
        try
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, false, q);

            var creditAgents = await _creditAgentService.GetCreditAgents(branchId, param);

            counts = creditAgents.Count;

            CreditAgents = creditAgents.Items.ToList();

            isLoading = false;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }


    private async Task LoadData(LoadDataArgs e)
    {
        int pageIndex = e.Skip!.Value;

        (sortColumn, sortDescending) = e.PageInfo("FirstName");

        var filterColumn = e.Filters?.FirstOrDefault(p =>
                                p.Property == nameof(VCreditAgent.FirstName) ||
                                p.Property == nameof(VCreditAgent.LastName) ||
                                p.Property == nameof(VCreditAgent.Phone) ||
                                p.Property == nameof(VCreditAgent.BranchName) ||
                                p.Property == nameof(VCreditAgent.EmailAddress)
                            )?.FilterValue.ToString() ?? string.Empty;

        _filter = filterColumn ?? string.Empty;

        await GetCreditAgents(BranchId, _filter);
    }

    protected async Task GetSearchChandged(string filter)
    {
        _filter = filter;
        await GetCreditAgents(BranchId, filter);
    }


    async Task SelectedBranchChanged()
    {
        await GetCreditAgents(BranchId, _filter);

        await InvokeAsync(StateHasChanged);
    }

    private async Task CloseTab(int closeTabIndex)
    {
        if (closeTabIndex == 1)
        {
            ShowAddedTab = false;

            await GetCreditAgents(BranchId, _filter);
        }

        selectedTabIndex = 0;

    }
    private async Task SelectedItemChanged(object? args)
    {
        if (args is not null)
        {
            selectedBranchId = ((BranchDto)args).BranchId;

            await GetCreditAgents(selectedBranchId, _filter);
        }
    }
}
