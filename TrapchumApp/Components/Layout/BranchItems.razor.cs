using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Layout
{
    public partial class BranchItems
    {
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string UserId { get; set; } = string.Empty;
        [Parameter] public bool BranchState { get; set; } = false;
        [Parameter] public bool CanOpenBranch { get; set; } = false;
        [Parameter] public bool IsAccountOnly { get; set; } = false;
    }
}