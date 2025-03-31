using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Responses.ViewModels.Vendors;
using Microsoft.AspNetCore.Components;
using Radzen;
using TrapchumApp.Components.Pages.Lists.OtherNames;

namespace TrapchumApp.Components.Pages.Lists.Customers
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
            var gResponse = await _gService.GetGuarantorsAsync(Guid.Empty); //_customerService.GetGuarantorsAsync();

            if (gResponse.IsSuccess)
            {
                Guarantors = gResponse.Data!;
            }
            else
            {
                Guarantors = [];
            }

            await base.OnInitializedAsync();
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