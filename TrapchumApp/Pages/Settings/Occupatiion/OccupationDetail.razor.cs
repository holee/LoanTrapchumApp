using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Queries.GetOccupationDetailsById;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;

namespace TrapchumApp.Pages.Settings.Occupatiion
{
    public partial class OccupationDetail
    {
        [Parameter] public int Id { get; set; }

        private OccupationDto? _occupation;

        private bool _isPageLoading = false;
        protected override async Task OnInitializedAsync()
        {
            await LoadOccupation();

            await base.OnInitializedAsync();
        }

        private async Task LoadOccupation()
        {

            _isPageLoading = true;

            var result = await mediator.Send(new GetOccupationDetailsByIdQuery(Id));

            _occupation = result.Value;

            _isPageLoading = false;
        }

        private async Task OnEditClick()
        {
            DialogService.Close(false);

            var result = await DialogService.OpenAsync<OccupationEdit>(
                "Edit Occupation",
                new Dictionary<string, object?> { { nameof(OccupationEdit.Id), Id } });

            if (result is OccupationDto updated)
            {
                _occupation = updated;

                StateHasChanged();
            }
        }
    }
}
