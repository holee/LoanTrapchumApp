using Applications.Exceptions;
using Applications.Extensions;
using Applications.Features.Notes.Command.DeleteNote;
using Applications.Features.Notes.Queries.GetNoteByCustomerId;
using Applications.Features.Notes.Queries.GetNoteById;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Identity;

namespace TrapchumApp.Pages.Lists.Customers.Notes
{
    public partial class CustomerNotes
    {

        private IEnumerable<VCustomerNotes> Notes = [];
        [Parameter] public string? CustomerId { get; set; } = string.Empty;
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool ShowCommand { get; set; } = false;
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        private readonly ILogger<CustomerNotes> _logger;

        private string? _userId = string.Empty;
        public CustomerNotes(ILogger<CustomerNotes> logger)
        {
            _logger = logger;
        }

        protected override async Task OnInitializedAsync()
        {


            var auth = await AuthenticationState;

            _userId = auth.User.GetId();

            await GetCustomerNotes(CustomerId);

            await base.OnInitializedAsync();
        }

        async Task AddNewNote()
        {
            var auth = await AuthenticationState;

            var userId = auth.User!.GetId();

            bool? dialogueResult = await DialogService.OpenAsync<CustomerNoteAdd>("New Note", new Dictionary<string, object?>
            {
                { "CustomerId", CustomerId },
                { "UserId", _userId }
            });

            if (dialogueResult is not null && dialogueResult == true)
            {
                await GetCustomerNotes(CustomerId);
            }

        }


        async Task GetCustomerNotes(string? customerId)
        {
            try
            {
                if (customerId.IsNullOrEmpty()) return;

                var rsp = await mediator.Send(new GetNotesByCustomerIdQuery(customerId));

                Notes = !rsp.IsError ? rsp.Value : [];

            }
            catch (MediatorException ex)
            {
                _logger.LogError(ex, "Error in GetCustomerNotes");

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            }

            StateHasChanged();
        }


        async Task GetEditNotes(VCustomerNotes note)
        {
            try
            {
                if (note.CustomerRefId.IsNullOrEmpty()) return;

                var rsp = await mediator.Send(new GetNoteByIdQuery(note.NoteId, note.CustomerRefId));

                var parameters = new Dictionary<string, object?>
                {
                    { "CustomerId", note.CustomerRefId },
                    { "UserId", _userId },
                    { "NoteId", note.NoteId }
                };

                await DialogService.OpenAsync<CustomerNoteAdd>("Edit", parameters);

            }
            catch (MediatorException ex)
            {
                _logger.LogError(ex, "Error in GetCustomerNotes");

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            }

            StateHasChanged();
        }



        async Task GetDetailNotes(VCustomerNotes note)
        {
            try
            {
                if (note.CustomerRefId.IsNullOrEmpty()) return;

                var rsp = await mediator.Send(new GetNoteByIdQuery(note.NoteId, note.CustomerRefId));

                var parameters = new Dictionary<string, object?>
                {
                    { "CustomerId", note.CustomerRefId },
                    { "UserId", _userId },
                    { "NoteId", note.NoteId }
                };

                await DialogService.OpenAsync<CustomerNoteAdd>("Edit", parameters);

            }
            catch (MediatorException ex)
            {
                _logger.LogError(ex, "Error in GetCustomerNotes");

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            }

            StateHasChanged();
        }


        async Task DeleteNotes(VCustomerNotes note)
        {
            try
            {
                if (note.CustomerRefId.IsNullOrEmpty()) return;

                var confirm = await DialogService.Confirm("Are you sure you want to delete this note?", "Delete Note", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });

                if (confirm == false) return;

                var rsp = await mediator.Send(new DeleteNoteCommand(note.NoteId, note.CustomerRefId));

                if (rsp.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description ?? "An error occurred while deleting the note.");

                    return;
                }

                NotificationService.Notify(NotificationSeverity.Success, "Note deleted successfully.");

                await GetCustomerNotes(CustomerId);
            }
            catch (MediatorException ex)
            {
                _logger.LogError(ex, "Error in GetCustomerNotes");

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            }

            StateHasChanged();
        }


        void OnRowRender(RowRenderEventArgs<VCustomerNotes> args)
        {
            if(args.Data is null) return;

            args.Data.RowIndex = args.Index + 1; // store 1-based index
        }

    }
}