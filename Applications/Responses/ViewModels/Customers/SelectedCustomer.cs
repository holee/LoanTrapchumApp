namespace Applications.Responses.ViewModels.Customers
{
    public class SelectedCustomer
    {
        public long? Id { get; set; }
        public string? AltName { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? IdCard { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int Circle { get; set; } = 0;

    }
}
