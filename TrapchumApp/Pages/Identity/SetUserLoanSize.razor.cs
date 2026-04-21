using Applications.Features.Settings.Requests;
using Applications.Features.Settings.UseCases;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Identity
{
    public partial class SetUserLoanSize
    {
        private readonly ISettingServiceImpl _settingService;
        [Parameter, EditorRequired] public string UserId { get; set; } = string.Empty;

        private UserLoanSizeDtoCreate model = new UserLoanSizeDtoCreate();

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


        async Task HandleFormSubmit()
        {

            var resp = await _settingService.CreateUserLoanSizeAsync(model);

            if (!resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);

                _toastService.ShowError("Loan Size", "Failed to set loan size");

                return;
            }

            _toastService.ShowSuccess("Loan Size", "Loan Size was seeting");

            DialogService.Close(true);

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