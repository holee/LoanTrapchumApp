using Applications.Features.Guarantor.Dtos;
using Applications.Features.Guarantor.Queries.GetGuarantorsAsPaging;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Lists;
using TrapchumApp.Pages.Lists.OtherNames.Guarantors;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class GuarantorModalCreate
    {
        private readonly IGuarantorService _gService;

        public GuarantorModalCreate(IGuarantorService gService)
        {
            _gService = gService;
        }

        IEnumerable<VGuarantor> Guarantors = [];
        [Parameter]
        public HashSet<VGuarantorsAvailable>? SelectedGuarantors { get; set; } = new();
        VGuarantorsAvailable? selectedGuarantor;
        bool isValid = false;
        private VGuarantorsAvailable guarantor = new();

        protected override async Task OnInitializedAsync()
        {
            await GetGuarantors();

            await base.OnInitializedAsync();
        }

        private async Task GetGuarantors()
        {
            var command = new GetGuarantorsAsPagingQuery(null, new QueryParam(0, 0));

            var rsp = await mediator.Send(command);  //_gService.GetGuarantorsAsync(string.Empty); //_customerService.GetGuarantorsAsync();

            Guarantors = rsp.Items;
        }

        Task<IEnumerable<VGuarantor>> SearchGuarantors(string? filter = null)
        {
            var guarantors = Guarantors.Where(x => x.GuarantorName!.ToLower().Contains(filter!.ToLower()));
            return Task.FromResult(guarantors);
        }

        void CloseModal()
        {
            DialogService.Close(false);
        }
        void AddSelectedGuarantor()
        {
            if (selectedGuarantor is not null)
            {
                SelectedGuarantors!.Add(selectedGuarantor);
                selectedGuarantor = null;
            }
            StateHasChanged();
        }
        void RemoveGuarantor(VGuarantorsAvailable g)
        {
            SelectedGuarantors!.Remove(g);
            StateHasChanged();
        }


        void SelectGuarantorChanged(VGuarantorsAvailable guarantorsAvailable)
        {
            selectedGuarantor = guarantorsAvailable!;
            Console.WriteLine(guarantorsAvailable!.Name);
        }

        async Task ShowGuarantorModal()
        {
            await DialogService.OpenAsync<GuarantorAddModal>("Guarantor");
        }

        bool IsSuccess = false;



    }
}