namespace Application.Calculations
{
    public sealed class Builders(int currencyId, int payEvery, int repaymentModeId, int collectionFeeModeId, int installmentTypeId, decimal effectiveRate, DateTimeOffset? firstPayment = null, DateTimeOffset? startDate = null)
    {
        public int CurrencyId { get; } = currencyId;
        public int PayEvery { get; } = payEvery;
        public int RepaymentModeId { get; } = repaymentModeId;
        public int CollectionFeeModeId { get; } = collectionFeeModeId;
        public int InstallmentTypeId { get; } = installmentTypeId;
        public decimal EffectiveRate { get; } = effectiveRate;
        public DateTimeOffset? FirstPayment { get; } = firstPayment ?? DateTimeOffset.UtcNow;
        public DateTimeOffset? StartDate { get; } = startDate ?? DateTimeOffset.UtcNow;
    }
}
