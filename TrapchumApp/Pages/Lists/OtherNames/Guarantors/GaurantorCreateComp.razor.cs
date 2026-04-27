using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;
using Applications.Features.AdditionalInformations.Queries.GetSalutaions;
using Applications.Features.Guarantor.Dtos;

namespace TrapchumApp.Pages.Lists.OtherNames.Guarantors
{
    public partial class GaurantorCreateComp
    {
        [Parameter] public EntityGuarantorCreate Model { get; set; } = new();
        [PersistentState]
        public IEnumerable<OccupationDto>? _occuptions { get; set; }

        private IEnumerable<SalutationDto> _salutations = [];
        protected override async Task OnInitializedAsync()
        {
            if (_occuptions is null)
            {
                await GetOccupations();

            }

            await GetSalutations();

            await base.OnInitializedAsync();
        }

        private async Task GetSalutations()
        {
            var command = new GetSalutationsQuery(null, new QueryParam(0, 0, string.Empty));

            var rsp = await mediator.Send(command);

            _salutations = rsp.Items;

            _filteredSalutations= rsp.Items;
        }

        private async Task GetOccupations()
        {
            var command = new GetOccupationsQuery(0, new QueryParam(0, 0, string.Empty), TypeOfEnum.Job);

            var rsp = await mediator.Send(command);

            _occuptions = rsp.Items;
        }

        void SetFirstName(string e)
        {
            var value = e ?? string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                var names = value.Split(" ");
                if (names.Length == 1)
                {
                    Model.FirstName = names[0];
                    Model.LastName = string.Empty;
                }
                if (names.Length == 2)
                {
                    Model.FirstName = names[0];
                    Model.LastName = names[1];
                }
            }
            else
            {
                Model.FirstName = string.Empty;
                Model.LastName = string.Empty;
            }

        }

    }
}