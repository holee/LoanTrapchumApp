namespace Application.Features.Activities.Command.CreateActivity
{
    public sealed class ActivityDtoCreate 
    {
        public int Id { get; set; }
        public string? CustomerRefId { get; set; }
        public DateTime? ActivityDate { get; set; } = DateTime.Now;
        public string? Description { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
