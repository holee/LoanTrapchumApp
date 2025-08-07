using Applications.Features.Customers.Requests;
using Applications.Features.Customers.UseCases;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerNoteAdd
    {
        private readonly AddCustomerNotesUseCase _custNoteAdd;

        public CustomerNoteAdd(AddCustomerNotesUseCase custNoteAdd)
        {
            _custNoteAdd = custNoteAdd;
        }

        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public string? UserId { get; set; }
        bool isBusy = false;
        private CustomerNoteDtoCreate model = new();

        async void FormSubmit()
        {
            isBusy = true;

            model.CustomerRefId = CustomerId;

            model.CreditAgentRefId = UserId;

            var noteResponse = await _custNoteAdd.ExecuteAsync(model);

            if (noteResponse.IsSuccess)
            {
                isBusy = false;

                DialogService.Close(true);
            }

        }
    }
}