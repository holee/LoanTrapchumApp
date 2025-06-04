using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Domains.Models.Addresses;

namespace TrapchumApp.Pages.Lists.Operations.Communes
{
    public partial class CommuneCreatePage
    {

        private readonly IDistrictService districtService;
        private readonly ICommuneService communeService;

        public CommuneCreatePage(IDistrictService districtService, ICommuneService communeService)
        {
            this.districtService = districtService;
            this.communeService = communeService;
        }
        protected List<District> districts = [];
        protected CommuneDtoCreate model = new CommuneDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            var results = await districtService.GetAllAsync(null);
            if (results.IsSuccess)
            {

                districts = results.Data!.ToList();
            }
            await base.OnInitializedAsync();
        }
        protected async Task Create()
        {
            try
            {

                var response = await communeService.CreateAsync(model);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "District new record was created successful.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Warning, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}