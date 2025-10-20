namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity
{
    public sealed class ActivityDtoEdit 
    {
        public int? Id { get; set; }
        public string? CustomerRefId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
