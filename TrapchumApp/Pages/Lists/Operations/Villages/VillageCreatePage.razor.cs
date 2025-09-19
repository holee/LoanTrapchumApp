using Applications.Features.Addresses.Responses;
using Applications.Interfaces.Addresses;
using Domains.Models.Addresses;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Operations.Villages
{
    public partial class VillageCreatePage
    {

        private readonly ICommuneService communeService;
        private readonly IVillageServer villageServer;

        public VillageCreatePage( ICommuneService communeService, IVillageServer villageServer)
        {
            this.communeService = communeService;
            this.villageServer = villageServer;
        }
        [Parameter] public int? Id { get; set; }
        protected List<Commune> communelist = [];
        protected VillageDtoCreate model = new VillageDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await communeService.GetAllAsync(null);
                if (results.IsSuccess)
                {

                    communelist = results.Data!.ToList();
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
                var response = await villageServer.CreateAsync(model);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Village new record was created successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }
    }
}