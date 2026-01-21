namespace Applications.Features.Guarantor.Dtos
{
    public class GuarantorDto
    {
        public int Id { get; set; } 
        public Guid? EntityNameRefId { get; set; }
        public string? GuarantorName { get; set; }
        public Guid? CustomerRefId { get; set; }
        public string? CustomerName { get; set; }
        public long? LoanRefId { get; set; }
    }
}
