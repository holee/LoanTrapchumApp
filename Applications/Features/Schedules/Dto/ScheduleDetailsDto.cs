namespace Applications.Features.Schedules.Dto
{
    public sealed class ScheduleDetailsDto 
    {
        public long LoanScheduleId { get; set; }
        public decimal Principle { get; set; } = 0;
        public decimal Interest { get; set; } = 0;
        public decimal CollectionFee { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public DateTime? PaymentDate { get; set; }
        public List<PaymentLineDetailsDto> PaymentLines { get; set; } = [];
    }
}
