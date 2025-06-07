using Applications.Interfaces.Addresses;
using Applications.Responses.ViewModels.Addresses;
using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices;

namespace TrapchumApp.Pages.Lists.Operations.Villages
{
    public partial class VillageDetailsPage
    {

        private readonly IVillageServer villageServer;
        public VillageDetailsPage(IVillageServer villageServer)
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
                var result = await villageServer.GetDettailsAsync(Id);
                if (result.IsSuccess && result.Data != null)
                {
                    model = result.Data;
                }
                else
                {
                    throw new Exception(result.ErrMessage ?? "Failed to load village details.");
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