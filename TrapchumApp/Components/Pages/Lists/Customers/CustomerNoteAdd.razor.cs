using Applications.Requests.Lists.Customers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerNoteAdd
    {
        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public Guid UserId { get; set; }
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