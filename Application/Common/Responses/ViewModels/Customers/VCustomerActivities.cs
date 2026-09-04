namespace Application.Responses.ViewModels.Customers
{
    public sealed class VCustomerActivities : BasePropEntity
    {
        public int ActivityId { get; set; } 
        public string? CustomerRefId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; } = false;
        public DateTime? ActivityDate { get; set; }
        public DateTimeOffset? CreatedDT { get; set; }
        public DateTimeOffset? ModifiedDT { get; set; }
    }
}
