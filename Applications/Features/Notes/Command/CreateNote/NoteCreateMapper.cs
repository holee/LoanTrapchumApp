using Domains.Models.Lists.Customers;

namespace Applications.Features.Notes.Command.CreateNote
{
    public static class NotEditMapper 
    {

        public static CustomerNote ToCreateNote(this AddNoteCommand command)
        {
            return new CustomerNote
            {
                CustomerRefId = command.CustomerRefId,
                CreditAgentRefId = command.CreditAgentRefId,
                NoteDate= command.NoteDate,
                Description = command.Description,
                EditSeq = 0
            };
        }
    }
}
