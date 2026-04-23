using Applications.Features.AdditionalInformations.Command.UpdateOccupation;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;
using Applications.Responses.Paginations;
using Radzen;

namespace TrapchumApp.Pages.Settings.Occupatiion
{
    public partial class OccupationEdit
    {
        [Parameter] public int Id { get; set; }

        private OccupationDtoUpate model = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadOccupation();
            await base.OnInitializedAsync();
        }

        private async Task LoadOccupation()
        {
            var param = new QueryParam(0, 1, nameof(OccupationDto.OccupationId), false, false, null);
            var result = await mediator.Send(new GetOccupationsQuery(Id, param));
            var item = result.Items?.FirstOrDefault();
            if (item is not null)
            {
                model = new OccupationDtoUpate
                {
                    OccupationId = item.OccupationId,
                    OccupationTitle = item.OccupationTitle,
                    Khmer = item.Khmer,
                    TypeOf = item.TypeOf,
                };
            }
        }

        private async Task OnSubmit(OccupationDtoUpate data)
        {
            var rsp = await mediator.Send(new UpdateOccupationCommand(model));

            if (rsp.IsError)
            {
                await DialogService.Alert(rsp.FirstError.Description, "Error", new ConfirmOptions { OkButtonText = "OK" });
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, "Success", "Occupation updated successfully.");
            DialogService.Close(rsp.Value);
        }
    }
}
