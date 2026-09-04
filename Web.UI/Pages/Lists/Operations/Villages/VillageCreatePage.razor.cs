using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Domains.Models.Addresses;

namespace Web.UI.Pages.Lists.Operations.Villages
{
    public partial class VillageCreatePage
    {

        private readonly IAddressService communeService;
        private readonly IAddressService villageServer;

        public VillageCreatePage(IAddressService villageServer)
        {
            this.communeService = villageServer;
            this.villageServer = villageServer;
        }
        [Parameter] public int? Id { get; set; }
        protected List<Commune> communelist = [];
        protected VillageDtoCreate model = new VillageDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await communeService.GetAllCommunesAsync(null);

                if (!results.IsError)
                {

                    communelist = results.Value.ToList();
                    model.CommuneRefId = Id ?? 0;
                }
            }
            catch(Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            
            await base.OnInitializedAsync();
        }
        protected async Task Create()
        {
            try
            {
                if(model.CommuneRefId==null || model.CommuneRefId < 0)
                {
                    NotificationService.Notify(NotificationSeverity.Warning, "Commune is empty! please select any commune.");
                    return;
                }
                var response = await villageServer.CreateVillageAsync(model);

                if (!response.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Village new record was created successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }
    }
}