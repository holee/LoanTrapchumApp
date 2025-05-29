using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class NameDetailModal
    {
        private readonly IOtherNameServiceImpl _otherNameService;

        public NameDetailModal(IOtherNameServiceImpl otherNameService)
        {
            _otherNameService = otherNameService;
        }

        VOtherNames? model = new();
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public EventCallback<string> OnEdit { get; set; } 
        [Parameter] public string? Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await GetDeatilsName(Id);
            await base.OnParametersSetAsync();
        }


        async Task CloseTabs()
        {
            await OnClose.InvokeAsync(false);
        }

        async Task OnClickEdit()
        {
            await OnEdit.InvokeAsync(Id);
        }

        async Task GetDeatilsName(string? id)
        {
            var rsp = await _otherNameService.GetDetailAsync<VOtherNames>(id);

            if (rsp.IsSuccess)
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