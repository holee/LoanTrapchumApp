using Applications.Exceptions;
using Applications.Extensions;
using Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteByCustomerId;
using Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteById;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using Domains.Models.Identity;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerNotes
    {

        [Parameter] public IEnumerable<VCustomerNotes> Notes { get; set; } = [];
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


            if (auth.User.Identity!.IsAuthenticated)
            {
                _userId = auth.User.GetId();
            }


            if (CustomerId.IsNotNullOrEmpty() && !Notes.Any())
            {
                await GetCustomerNotes(CustomerId);
            }

            await base.OnInitializedAsync();
        }

        async Task AddNewNote()
        {
            var auth = await AuthenticationState;

            if (auth.User.Identity!.IsAuthenticated == false)
            {
                return;
            }

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


        async Task GetDeleteNotes(VCustomerNotes note)
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


        void OnRowRender(RowRenderEventArgs<VCustomerNotes> args)
        {
            args.Data.RowIndex = args.Index + 1; // store 1-based index
        }

    }
}