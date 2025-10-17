namespace Applications.Responses.ViewModels.Customers
{
    public sealed class VCustomerActivities 
    {
        public int ActivityId { get; set; } 
        public string? CustomerRefId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; } = false;
        public DateTime? ActivityDate { get; set; }
        public DateTime? CreatedDT { get; set; }
        public DateTime? ModifiedDT { get; set; }
    }
}
