using Applications.Features.Customers.Commands.NotesCommand.CreateNoteCommand;
using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.Commands.NotesCommand.EditNoteCommand
{
    public static class NoteEditMapper
    {

        public static CustomerNote ToEditNote(this EditNoteCommand command) => new CustomerNote
        {
            NoteId = command.NoteId,
            CustomerRefId = command.CustomerRefId,
            CreditAgentRefId = command.CreditAgentRefId,
            NoteDate = command.NoteDate,
            Description = command.Description,
        };

        public static EditNoteCommand ToEditNote(this AddNoteCommand command) => new EditNoteCommand
        {
            NoteId = command.NoteId,
            CustomerRefId = command.CustomerRefId,
            NoteDate = command.NoteDate ?? System.DateTime.Now,
            CreditAgentRefId = command.CreditAgentRefId,
            Description = command.Description,
        };

    }
}
