using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using TCLMS.Shared.Entities.Dtos.Lists;
using TCLMS.Shared.Entities.Dtos.Lists.Entities;
using TCLMS.Shared.Entities.ViewModels.Customers;
namespace TCLMS.Front.Pages.CreditAgents;

public partial class CreditAgent
{
    protected bool includeInactive = false;
    protected IEnumerable<EntEmployeeDto> Employees { get; set; } = new List<EntEmployeeDto>();
    protected List<VCreditAgent>? CreditAgents { get; set; }

    protected string? searchText = "";
    [CascadingParameter]
    Task<AuthenticationState> AuthState { get; set; } = default!;

    List<BranchDto> Branches = [];
    protected string? UserId { get; set; }
    int pageIndex = 0;
    int pageSize =10;
    string sortColumn = "FirstName";
    bool sortDescending = false;
    int counts = 0;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var branchResponse = await _branchService.GetBranchesAsync(0, 0, false, string.Empty);

            if (branchResponse.IsSuccess)
            {
                Branches = branchResponse.Data!.ToList();
                Branches.Add(new BranchDto
                {
                    BranchId = 0,
                    BranchName = "All"
                });
                BranchId = Branches.Last().BranchId;
            }
            await GetCreditAgents(searchText);
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }

        await base.OnInitializedAsync();
    }

    protected async Task GetCreditAgents(string? q = null)
    {
        try
        {
            isLoading = true;
            var creditAgents = await _creditAgentService.GetCreditAgents(pageIndex, pageSize, sortColumn, sortDescending, false, q);
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
        if (!string.IsNullOrEmpty(e.OrderBy))
        {
            sortColumn = e.OrderBy.Split(" ")[0];
            string dir = e.OrderBy.Split(" ")[1];
            sortDescending = dir.Contains("asc") ? false : true;
        }
        await GetCreditAgents(searchText);
    }

    protected async Task GetSearchChandged()
    {
        await GetCreditAgents(searchText);
    }

    async Task SelectedPageChanged()
    {
        await GetCreditAgents(searchText);
    }

    async Task SelectedBranchChanged()
    {
        if (BranchId == 0)
        {
            await GetCreditAgents(searchText);
        }
        else
            await GetCreditAgents(BranchId.ToString());
        await InvokeAsync(StateHasChanged);
    }

}
