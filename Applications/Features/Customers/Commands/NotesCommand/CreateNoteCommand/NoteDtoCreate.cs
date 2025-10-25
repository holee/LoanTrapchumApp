namespace Applications.Features.Customers.Commands.NotesCommand.CreateNoteCommand;

public class NoteDtoCreate
{
    public string? CustomerRefId { get; set; }
    public string? CreditAgentRefId { get; set; }
    public string? Description { get; set; }    
}
