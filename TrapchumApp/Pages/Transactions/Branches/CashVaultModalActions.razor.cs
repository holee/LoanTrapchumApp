using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchByUser;
using Applications.Features.Branches.Queries.GetBranchsByUser;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCashVault;
using Domains.Models.Identity;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Branches;

public partial class CashVaultModalActions
{
    [CascadingParameter]
    Task<AuthenticationState> AuthState { get; set; } = default!;
    [CascadingParameter] CascadingAppState bState { get; set; } = default!;
    [Parameter] public int BranchId { get; set; }
    [Parameter] public int BankId { get; set; } = 11;
    [PersistentState] public string? UserId { get; set; }
    [PersistentState] public IEnumerable<VCashVaults>? vCashVaults { get; set; }

    protected IEnumerable<BankResponse>? bankList = [];

    private IEnumerable<VBranch> Branches = [];

    private BankResponse? bank;

    private BranchDto? branch;

    private int? CurrencyId;

    private bool IsDisabled = false;

    DialogOptions options = new DialogOptions
    {
        ShowTitle = false,
        Width = "100vw",
        Height = "100vh",
        AutoFocusFirstElement = false,
    };
    protected override async Task OnInitializedAsync()
    {
        try
        {

            if (string.IsNullOrEmpty(UserId))
            {
                var user = (await AuthState).User;

                UserId = user.GetId();

                await bState.GetAuthenticatedUser(user);
            }

            await GetCashVault();

        }
        catch (Exception ex)
        {
            NotificationService!.Notify(NotificationSeverity.Error, ex.Message);
        }
        await base.OnInitializedAsync();
    }

    private async Task GetBranchByUser(string? userId)
    {
        var command = new GetBranchsByUserQuery(userId, 0);

        var bResponse = await mediator.Send(command);

        if (bResponse.IsSuccess())
        {
            Branches = bResponse.Value;
        }
    }

    private async Task GetCashVault()
    {
        var command1 = new GetCashVaultQuery(BranchId);

        var resp = await mediator.Send(command1);

        vCashVaults = resp.Items;
    }

    protected async void OnCashVaultOut()
    {

        var param = new Dictionary<string, object?> {
                { "BranchId", BranchId },
                { "BankId", BankId },
            };

        var resultModal = await DialogService.OpenAsync<VaultOutModal>("Vault Out", param, options);

    }
    protected async void OnCashVaultIn()
    {

        var param = new Dictionary<string, object?> {
                { "BranchId", BranchId },
                { "BankId", BankId },
            };

        var resultModal = await DialogService.OpenAsync<VaultInModal>("Vault In", param, options);

        if (resultModal != null)
        {

        }
    }
}