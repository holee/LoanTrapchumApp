using Applications.Features.OtherNames;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class ConvertToVendor
    {
        [Parameter] public string? Id { get; set; } = string.Empty;


        private readonly IOtherNameServiceImpl _otherNameService;
        public ConvertToVendor(IOtherNameServiceImpl otherNameService)
        {
            _otherNameService = otherNameService;
        }

        VOtherNames? name = new VOtherNames();

        ConvertCreateVendor model = new ConvertCreateVendor();

        protected override async Task OnParametersSetAsync()
        {
            await GetName(Id);

            await base.OnParametersSetAsync();
        }

        async Task HandleFormSubmit()
        {
            var rsp = await _otherNameService.ConvertToVendorAsync(Id, model);
            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Success", rsp.Data);

                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", rsp.ErrMessage);
            }
        }

        async Task GetName(string? id)
        {
            try
            {
                var rsp = await _otherNameService.GetDetailAsync<VOtherNames>(id);

                if (rsp.IsSuccess)
                {
                    name = rsp.Data!;
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error", rsp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }
        }
    }
}