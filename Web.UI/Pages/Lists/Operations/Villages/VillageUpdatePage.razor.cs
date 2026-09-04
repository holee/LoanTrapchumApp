using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Domains.Models.Addresses;
using Mapster;

namespace Web.UI.Pages.Lists.Operations.Villages
{
    public partial class VillageUpdatePage
    {
        private readonly IAddressService communeService;
        private readonly IAddressService villageServer;
        [Parameter] public int Id { get; set; }
        public VillageUpdatePage(IAddressService villageServer)
        {
            this.communeService = villageServer;
            this.villageServer = villageServer;
        }
        protected List<Commune> communelist = [];
        protected VillageDtoUpdate model = new VillageDtoUpdate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await communeService.GetAllCommunesAsync(null);
                if (!results.IsError)
                {

                    communelist = results.Value.ToList();
                }
                var response = await villageServer.GetVillageByIdAsync(Id);

                if (!response.IsError)
                {
                    model = response.Value.Adapt<VillageDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Village", response.FirstError.Description);
                }
            } catch (Exception ex) {
                throw new Exception();
            }
           
            await base.OnInitializedAsync();
        }
        protected async Task Update()
        {
            try
            {

                var response = await villageServer.UpdateVillageAsync(model);

                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Village record was updated successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}