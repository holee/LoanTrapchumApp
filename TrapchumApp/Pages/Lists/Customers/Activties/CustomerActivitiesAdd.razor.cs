using Applications.Commons.Messages.Activities;
using Applications.Exceptions;
using Applications.Features.Activities.ActitvityQuery.GetActivityById;
using Applications.Features.Activities.Command.CreateActivity;
using Applications.Features.Activities.Command.EditActivity;

namespace TrapchumApp.Pages.Lists.Customers.Activties
{
    public partial class CustomerActivitiesAdd
    {
        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public int? ActivityId { get; set; } = 0;

        private ActivityDtoCreate _activity = new ActivityDtoCreate();
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            if (auth.User.Identity!.IsAuthenticated)
            {
                _activity.CustomerRefId = CustomerId;
            }

            if (ActivityId > 0)
            {
                await GetActivityDetails(ActivityId);
            }


            await base.OnInitializedAsync();
        }

        async Task GetActivityDetails(int? id)
        {

            if (!id.HasValue && id <= 0)
            {
                return;
            }

            var req = new GetActivityByIdQuery(ActivityId, CustomerId);

            var rsp = await mediator.Send(req);

            if (!rsp.IsError)
            {
                _activity = new ActivityDtoCreate
                {
                    Id = rsp.Value.ActivityId,
                    Description = rsp.Value.Description,
                    CustomerRefId = rsp.Value.CustomerRefId,
                    Status = rsp.Value.Status,
                    ActivityDate = rsp.Value.ActivityDate
                };
            }

            // To be implemented if needed
        }


        async Task HandleFormSubmit()
        {

            try
            {

                if (_activity.Id > 0)
                {
                    var rsp = await mediator.Send(new EditActivityCommand(_activity));

                    if (!rsp.IsError)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, ActivityMessage.UpdateSuccess);
                        DialogService.Close(true);
                        return;
                    }

                }
                else
                {
                    var rsp = await mediator.Send(new CreateActivityCommand(_activity));

                    if (!rsp.IsError)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, ActivityMessage.CreateSuccess);
                        
                        DialogService.Close(true);
                       
                        return;
                    }

                }
            }
            catch (MediatorException ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }

        }


    }
}