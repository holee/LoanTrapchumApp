using Applications.Features.Loans.Requests;

namespace Applications.Calculations
{
    public sealed class LoanInfo
    {
        public LoanInfo(int? payEvery, decimal? effectiveRate, int? collectionFeeModeId, int? installmentTypeId, DateTimeOffset? firstPayment = null, DateTimeOffset? startDate = null)
        {
            PayEvery = payEvery;

            CollectionFeeRate = effectiveRate;

            FirstPayment = firstPayment ?? DateTimeOffset.UtcNow;

            StartDate = startDate ?? DateTimeOffset.UtcNow;

            CollectionFeeModeId = collectionFeeModeId;

            InstallmentTypeId = installmentTypeId;
        }

        public LoanInfo(LoanDtoCreate req)
        {
            PayEvery = req.PayEvery;

            CollectionFeeRate = req.CollectionFeeRate;

            FirstPayment = req.FirstPaymentDate ?? DateTimeOffset.UtcNow;

            StartDate = req.StartDate ?? DateTimeOffset.UtcNow;

            CollectionFeeModeId = req.CollectionFeeModeId;

            InstallmentTypeId = req.InstallmentTypeId;
        }

        public int? PayEvery { get; init; }
        public decimal? CollectionFeeRate { get; init; } = 0;
        public DateTimeOffset? FirstPayment { get; init; }
        public DateTimeOffset? StartDate { get; init; }
        public int? CollectionFeeModeId { get; init; } = 1;
        public int? InstallmentTypeId { get; init; } = 1;
    }

}
