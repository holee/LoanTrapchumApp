namespace Applications.Calculations;

public sealed record PaymentDtoCreate
{

    public PaymentDtoCreate(decimal? recievePrincipleAmount, decimal? recieveInterestAmount, decimal? recievecollectionFeeAmount, decimal? penaltyAmountAmount)
    {
        RecieveInterestAmount = recieveInterestAmount;
        RecieveCollectionFeeAmount = recievecollectionFeeAmount;
        PenaltyAmountAmount = penaltyAmountAmount;
        RecievePrincipleAmount = recievePrincipleAmount;
    }

    public PaymentDtoCreate()
    {
        RecieveInterestAmount = 0.00M;
        RecieveCollectionFeeAmount = 0.00M;
        PenaltyAmountAmount = 0.00M;
        RecievePrincipleAmount = 0.00M;
    }

    public decimal? RecievePrincipleAmount { get; set; }
    public decimal? RecieveInterestAmount { get; set; }
    public decimal? RecieveCollectionFeeAmount { get; set; }
    public decimal? PenaltyAmountAmount { get; set; }
    public decimal? TotalPaymentWithPenalty => RecievePrincipleAmount + RecieveInterestAmount + RecieveCollectionFeeAmount + PenaltyAmountAmount;
    public decimal? TotalPaymentNoPenalty => RecievePrincipleAmount + RecieveInterestAmount + RecieveCollectionFeeAmount;
}


