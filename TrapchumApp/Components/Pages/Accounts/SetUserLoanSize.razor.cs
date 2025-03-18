using Applications.Interfaces.setting;
using Applications.Requests.SysSettings;
using Domains.Models.Loans;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Accounts
{
    public partial class SetUserLoanSize
    {
        private readonly ISettingService _settingService;

        public SetUserLoanSize(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [Parameter, EditorRequired] public string UserId { get; set; } = string.Empty;

        UserLoanSizeDtoCreate model = new UserLoanSizeDtoCreate();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        async void HandleFormSubmit()
        {


            var loanSize = model.Adapt<UserLoanSize>();

            var resp = await _settingService.ApplyUserLoanSizeAsync(loanSize);

            if (!resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, "Loan Size was seeting");
            }
        }
    }
}