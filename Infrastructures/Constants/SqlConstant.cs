namespace Infrastructures.Constants;

public static class SqlConstant
{
    public const string GetLoanResponse = @"
            SELECT l.ContractNumber,
                l.BranchName,
                l.AltName,
                l.Phone,
                l.Salutation,
                l.RegisterDate,
                l.CreditAgentKhmer,
                l.AgentPhone,
                l.FirstPaymentDate,
                l.CurrencyId,
                l.Principle,
                l.InterestRate,
                l.OutStanding,
                l.LastPaymentDate,
                l.Address,
                l.Circle,
                l.PeriodName
            FROM VLoansDisbursments l
            WHERE l.BranchId=@BranchId AND l.LoanId=@LoanId;
            SELECT 
                LoanScheduleId,
                CurrencyId,
                InstallmentNo,
                PaymentDate,
                TotalDay,
                Principle,
                Interest,
                CollectionFee,
                TotalPay,
                LoanOutstanding,
                ScheduleNum
            FROM VSchedules 
            WHERE BranchId=@BranchId AND LoanId=@LoanId;";



    public const string GetLoanResponseByScheduleNum = @" 
            SELECT 
            l.ContractNumber,
            l.BranchName,
            l.AltName,
            l.Phone,
            l.Salutation,
            l.RegisterDate,
            l.CreditAgentKhmer,
            l.AgentPhone,
            l.FirstPaymentDate,
            l.CurrencyId,
            l.Principle,
            l.InterestRate,
            l.OutStanding,
            l.LastPaymentDate,
            l.Address,
            l.Circle,
            l.PeriodName
        FROM 
            VLoansDisbursments l
        WHERE l.BranchId=@BranchId AND l.LoanId=@LoanId;

        SELECT 
            LoanScheduleId,
            CurrencyId,
            InstallmentNo,
            PaymentDate,
            TotalDay,
            Principle,
            Interest,
            CollectionFee,
            TotalPay,
            LoanOutstanding,
            ScheduleNum
        FROM 
            VSchedules 
        WHERE BranchId=@BranchId AND LoanId=@LoanId AND ScheduleNum=@ScheduleNum;";




    public const string CustomerInActiveLoan = @"SELECT 
                            CustomerId,
                            FirstName,
                            LastName,
                            IsLoanActive 
                     FROM VCustomerList 
                     WHERE CustomerId=@CustomerId AND IsLoanActive=@IsLoanActive";




    public const string CustomerAlreadyRequest = @"SELECT LoanRequestId,RequestAmount,CustomerId 
                        FROM VLoanRequests 
                        WHERE CustomerId=@CustomerId AND 
                              StatusId NOT IN (4)";


}
