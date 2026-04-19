using Domains.Shared.Enums;
using System.Linq.Expressions;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Command.CreateOccupation;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;
using Applications.Responses.Paginations;

namespace TrapchumApp.Pages.Lists
{
    public partial class OccupationDropDown
    {
        [Parameter] public TypeOfEnum TypeOf { get; set; } = TypeOfEnum.Job;
        [Parameter] public string Id { get; set; } = $"{Guid.NewGuid().ToString("N")}";
        [Parameter] public string Name { get; set; } = $"{Guid.NewGuid().ToString("N")}";
        [Parameter] public string Value { get; set; } = string.Empty;
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool AllowClear { get; set; } = false;
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public Expression<Func<string>> ValueExpression { get; set; } = default!;

        IEnumerable<OccupationDto> Occuptions { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {

            await GetOccupations();

            await base.OnInitializedAsync();
        }


        async Task AddOccupation()
        {
            if (string.IsNullOrEmpty(Value)) return;

            var confirm = await DialogService.Confirm("Are you sure you want to add this occupation?", "Confirm", new ConfirmOptions
            {
                OkButtonText = "Yes",
                CancelButtonText = "No"
            });

            if (confirm == false) return;

            var occupation = new OccupationDtoCreate
            {
                OccupationTitle = Value,

                Khmer = string.Empty,

                TypeOf = TypeOfEnum.Position.ToString()
            };

            var rsp = await mediator.Send(new CreateOccupationCommand(occupation)); 

            if (!rsp.IsError)
            {
                Value = string.Empty;
            }
            else
            {
                await GetOccupations();
            }

        }

        private async Task GetOccupations(string? filter = null)
        {

            Tracker.Show();

            var rsp = await mediator.Send(new GetOccupationsQuery(0, new QueryParam(0, 0, string.Empty, filter), TypeOf)); 

            Occuptions = rsp.Items;

            Tracker.Hide();
        }

    }
}