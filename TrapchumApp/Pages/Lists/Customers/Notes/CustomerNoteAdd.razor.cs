using Domains.Models.Identity;
using Applications.Features.Notes.Command.CreateNote;
using Applications.Features.Notes.Command.EditNote;
using Applications.Features.Notes.Queries.GetNoteById;
namespace TrapchumApp.Pages.Lists.Customers.Notes
{
    public partial class CustomerNoteAdd
    {
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public string? UserId { get; set; }
        [Parameter] public int NoteId { get; set; } = 0;
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; } = default!;

        bool isBusy = false;

        private AddNoteCommand model = new();

        protected override async Task OnInitializedAsync()
        {

            var auth = await authenticationStateTask;

            if (auth.User.Identity!.IsAuthenticated)
            {
                model.CreditAgentRefId = auth.User.GetId();
            }
            model.CustomerRefId = CustomerId;

            await GetEditNotes();

            await base.OnInitializedAsync();
        }



        async Task GetEditNotes()
        {
            if (NoteId > 0)
            {

                var query = new GetNoteByIdQuery(NoteId, CustomerId);

                var noteRsp = await mediator.Send(query);

                if (!noteRsp.IsError)
                {
                    NoteId = noteRsp.Value.NoteId;
                    model = new AddNoteCommand
                    {
                        NoteId = noteRsp.Value.NoteId,
                        NoteDate = noteRsp.Value.NoteDate,
                        CustomerRefId = noteRsp.Value.CustomerRefId,
                        CreditAgentRefId = UserId,
                        Description = noteRsp.Value.Description,
                    };
                }
            }

        }


        async Task<bool> CreateNote()
        {
            var noteRsp = await mediator.Send(model);

            if (noteRsp.IsError)
            {
                return false;
            }

            return true;

        }

        async Task<bool> EditNote()
        {

            var editNote = model.ToEditNote();

            var noteRsp = await mediator.Send(editNote);

            if (noteRsp.IsError)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = noteRsp.FirstError.Description,
                    Duration = 4000
                });
                return false;
            }

            return true;
        }


        async Task FormSubmit()
        {
            isBusy = true;

            if (NoteId > 0 && await EditNote())
            {

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = "Note updated successfully.",
                    Duration = 4000
                });

                DialogService.Close(true);
            }

            if (NoteId <= 0 && await CreateNote())
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = "New Note created successfully.",
                    Duration = 4000
                });

                DialogService.Close(true);
            }

            isBusy = false;
        }


    }
}