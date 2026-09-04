using Applications.Interfaces.Addresses;
using Applications.Responses.ViewModels.Addresses;

namespace Web.UI.Pages.Lists.Operations.Villages
{
    public partial class VillageDetailsPage
    {

        private readonly IAddressService villageServer;
        public VillageDetailsPage(IAddressService villageServer)
        {
            this.villageServer = villageServer;
            // Constructor logic if needed
        }
        [Parameter] public int Id { get; set; }
        public VVillageDetails model =new VVillageDetails();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await villageServer.GetVillageDetailsAsync(Id);
                if (!result.IsError)
                {
                    model = result.Value;
                }
                else
                {
                    throw new Exception(result.FirstError.Description);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Error initializing village details: {ex.Message}", ex);
                // Handle exception, e.g., log it or show a notification
            }
            await base.OnInitializedAsync();
        }
    }
}