using Applications.Features.Settings.Requests;
using Applications.Features.Settings.UseCases; 
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Identity
{
    public partial class SetUserLoanSizeEdit
    {
        [Parameter] public long Id { get; set; } = 0;
        [Parameter] public string? UserId { get; set; }

        private readonly ISettingServiceImpl _settingService;

        public SetUserLoanSizeEdit(ISettingServiceImpl settingService)
        {
            _settingService = settingService;
        }

        protected override async Task OnInitializedAsync()
        {
            await GetUserLoanSize(Id, UserId);

            await base.OnInitializedAsync();
        }

        UserLoanSizeDtoCreate model = new UserLoanSizeDtoCreate();

        async Task HandleFormSubmit()
        {

            var resp = await _settingService.EditUserLoanSizeAsync(model);

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


        private async Task GetUserLoanSize(long id,string? userId)
        {
            if (id <=0 ) return;

            var rsp = await _settingService.GetEditLoanSizeByUserId(id,userId);

            if (rsp.IsSuccess && rsp.Data is not null)
            {
                model = rsp.Data;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }
    }
}