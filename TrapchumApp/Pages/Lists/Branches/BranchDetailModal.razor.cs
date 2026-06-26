using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchDetailsById;

namespace TrapchumApp.Pages.Lists.Branches;

public partial class BranchDetailModal
{
    [Parameter] public int Id { get; set; }
    [Parameter] public PageType PageType { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    VBranch? branch = new();
    protected override async Task OnInitializedAsync()
    {

        var query = new GetBranchDetailsByIdQuery(Id);

        var result = await mediator.Send(query);  //_branchService.GetBranchById<VBranch>(Id);

        if (result.IsSuccess())
        {
            branch = result.Value;
        }

        await base.OnInitializedAsync();
    }
}