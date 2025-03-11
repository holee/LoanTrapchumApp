namespace Domains.Models.Loans
{
    public sealed class LoanInfo
    {
        public LoanInfo(int payEvery, decimal effectiveRate, int collectionFeeModeId, int installmentTypeId, DateTime firstPayment, DateTime startDate)
        {
            PayEvery = payEvery;
            EffectiveRate = effectiveRate;
            FirstPayment = firstPayment;
            StartDate = startDate;
            CollectionFeeModeId = collectionFeeModeId;
            InstallmentTypeId = installmentTypeId;
        }

        public int PayEvery { get; init; }
        public decimal EffectiveRate { get; init; } = 0;
        public DateTime FirstPayment { get; init; }
        public DateTime StartDate { get; init; }
        public int CollectionFeeModeId { get; init; } = 1;
        public int InstallmentTypeId { get; init; } = 1;
    }

}
