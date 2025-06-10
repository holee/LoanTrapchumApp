namespace Infrastructures.Constants.Procedures
{
    public static class ProcedureConstant
    {
        public const string SpGetCustomerDetailsById = nameof(SpGetCustomerDetailsById);
        public const string GetCustomerDetailById = "spGetCustomerDetailById";
        public const string GetLoanById = "spGetLoanById";//spGetLoanById
        public const string GetScheduleById = "spGetScheduleById";
        public const string GetCurrencyNotes = "getCurrencyNotes";
        public const string GetCurrencyNotesByBranchId = "GetCurrencyNotesByBranchId";
        public const string SpGetSchedulesByLoanId = nameof(SpGetSchedulesByLoanId);
        public const string SpGetLoansByBranchId = nameof(SpGetLoansByBranchId);
        public const string SpGetVSchedulesByPaymentDate = nameof(SpGetVSchedulesByPaymentDate);
        public const string SpGetLateVSchedules = nameof(SpGetLateVSchedules);
        public const string SpGetGaurantorWithLoans = nameof(SpGetGaurantorWithLoans);
        /// <summary>
        /// Employee Store Procedures
        /// </summary>
        public const string SpGetEmployeeByIdWithLoans = nameof(SpGetEmployeeByIdWithLoans);
        public const string SpGetLoansByEmployeeId = nameof(SpGetLoansByEmployeeId);
        public const string SpGetLoanRequestsByEmployeeId = nameof(SpGetLoanRequestsByEmployeeId);


    }
}
