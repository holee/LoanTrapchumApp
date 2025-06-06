using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Domains.Models.Addresses;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Operations.Villages
{
    public partial class VillageUpdatePage
    {
        private readonly ICommuneService communeService;
        private readonly IVillageServer villageServer;
        [Parameter] public int Id { get; set; }
        public VillageUpdatePage(ICommuneService communeService, IVillageServer villageServer)
        {
            this.communeService = communeService;
            this.villageServer = villageServer;
        }
        protected List<Commune> communelist = [];
        protected VillageDtoUpdate model = new VillageDtoUpdate();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await communeService.GetAllAsync(null);
                if (results.IsSuccess)
                {

                    communelist = results.Data!.ToList();
                }
                var response = await villageServer.GetByIdAsync(Id);

                if (response.IsSuccess)
                {
                    model = response.Data!.Adapt<VillageDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Village", response.ErrMessage);
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

                var response = await villageServer.UpdateAsync(model);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Village record was updated successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}