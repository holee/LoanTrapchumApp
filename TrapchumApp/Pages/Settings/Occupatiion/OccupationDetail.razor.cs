using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;

namespace TrapchumApp.Pages.Settings.Occupatiion
{
    public partial class OccupationDetail
    {
        [Parameter] public int Id { get; set; }

        private OccupationDto? _occupation;

        protected override async Task OnInitializedAsync()
        {
            await LoadOccupation();
            await base.OnInitializedAsync();
        }

        private async Task LoadOccupation()
        {
            var param = new QueryParam(0, 1, nameof(OccupationDto.OccupationId), false, false, null);
            var result = await mediator.Send(new GetOccupationsQuery(Id, param));
            _occupation = result.Items?.FirstOrDefault();
        }

        private async Task OnEditClick()
        {
            DialogService.Close(false);
            var result = await DialogService.OpenAsync<OccupationEdit>(
                "Edit Occupation",
                new Dictionary<string, object> { { nameof(OccupationEdit.Id), Id } });

            if (result is OccupationDto updated)
            {
                _occupation = updated;
                StateHasChanged();
            }
        }
    }
}
