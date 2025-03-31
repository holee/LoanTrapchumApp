namespace Applications.Responses.ViewModels.Customers
{
    public sealed class VCustomerActivities 
    {
        public int ActivityId { get; set; } 
        public string? CustomerRefId { get; set; }
        public bool Status { get; set; } = false; 
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedDT { get; set; }
        public DateTime? ModifiedDT { get; set; }
    }
}
