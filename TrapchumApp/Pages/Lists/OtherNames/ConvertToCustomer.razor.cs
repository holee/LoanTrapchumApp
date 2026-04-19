using Applications.Features.OtherNames.Dto;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class ConvertToCustomer
    {
        [Parameter] public string? Id { get; set; } = string.Empty;


        private readonly IOtherNameServiceImpl _otherNameService;
        private readonly IDepartmentServiceImpl _departmentService;
        public ConvertToCustomer(IOtherNameServiceImpl otherNameService, IDepartmentServiceImpl departmentService)
        {
            _otherNameService = otherNameService;
            _departmentService = departmentService;
        }
        VOtherNames? name = new VOtherNames();
        ConvertCreateCustomer model = new ConvertCreateCustomer();
        protected override async Task OnParametersSetAsync()
        {
            await GetName(Id);
            await base.OnParametersSetAsync();
        }

        async Task HandleFormSubmit()
        {
            var rsp = await _otherNameService.ConvertToCustomerAsync(Id, model);
            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Success", rsp.Data);
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", rsp.ErrMessage);
            }
        }


        async Task GetName(string? id)
        {
            try
            {
                var rsp = await _otherNameService.GetDetailAsync<VOtherNames>(id);

                if (rsp.IsSuccess)
                {
                    name = rsp.Data!;
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error", rsp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }
        }
    }
}