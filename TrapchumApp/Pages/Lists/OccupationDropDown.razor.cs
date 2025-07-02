using Applications.Requests.Settings;
using Applications.Responses.Lists.Entities;
using Applications.Services.Lists;
using Domains.Shared.Enums;
using System.Linq.Expressions;

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

        private readonly IOccupationServiceImpl _occupationService;
        IEnumerable<OcupationResponse> Occuptions { get; set; } = [];
        public OccupationDropDown(IOccupationServiceImpl occupationService)
        {
            _occupationService = occupationService;
        }

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

            var rsp = await _occupationService.CreateAsync(occupation);

            if (!rsp.IsSuccess)
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
            var rsp = await _occupationService.GetOccupations<OcupationResponse>(0, TypeOf, filter);

            Occuptions = rsp.Items ?? [];
        }

    }
}