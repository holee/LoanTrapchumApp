using Applications.Features.Settings.UseCases;
using Applications.Requests.Settings;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Identity
{
    public partial class SetUserLoanSize
    {
        private readonly ISettingServiceImpl _settingService;

        public SetUserLoanSize(ISettingServiceImpl settingService)
        {
            _settingService = settingService;
        }

        protected override void OnInitialized()
        {
            model.UserRefId = UserId;
            model.StartDate = DateTime.Now;
            base.OnInitialized();
        }
        [Parameter, EditorRequired] public string UserId { get; set; } = string.Empty;

        UserLoanSizeDtoCreate model = new UserLoanSizeDtoCreate();

        async Task HandleFormSubmit()
        {

            var resp = await _settingService.CreateUserLoanSizeAsync(model);

            if (!resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
                return;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, "Loan Size was seeting");
                DialogService.Close(true);
            }
        }
        private void StartDateChanged(DateTime? args)
        {
            if (args is not null && args.HasValue)
            {

                var newDate = model.EndDate ?? DateTime.Now;

                model.EndDate = newDate.AddYears(1);
            }
        }

    }
}