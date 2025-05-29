using Applications.Requests.Lists.Customers;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerNoteAdd
    {
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public string? UserId { get; set; }
        bool isBusy = false;
        private CustomerNoteDtoCreate model = new();

        async void FormSubmit()
        {
            isBusy = true;

            model.CustomerRefId = CustomerId;

            model.CreditAgentRefId = UserId;

            var noteResponse = await _customerService.AddCusomerNoteAsync(model);

            if (noteResponse.IsSuccess)
            {
                isBusy = false;

                DialogService.Close(false);
            }

        }
    }
}