namespace Infrastructures.Constants.Procedures
{
    public static class ProcedureConstant
    {

        /// <summary>
        /// Guarantor
        /// </summary>
        public const string SpGetGaurantorWithLoans = nameof(SpGetGaurantorWithLoans);
        public const string SpGetGaurantorWithLoanRequests = nameof(SpGetGaurantorWithLoanRequests);


        /// <summary>
        /// 
        /// </summary>
        public const string SpGetCustomerDetailsById = nameof(SpGetCustomerDetailsById);
        public const string GetCustomerDetailById = "spGetCustomerDetailById";
        public const string GetLoanById = "spGetLoanById";//spGetLoanById
        public const string GetScheduleById = "spGetScheduleById";
        public const string GetCurrencyNotes = "getCurrencyNotes";
        public const string GetCurrencyNotesByBranchId = "GetCurrencyNotesByBranchId";
        public const string SpGetSchedulesByLoanId = nameof(SpGetSchedulesByLoanId);
        public const string SpGetLoansByBranchId = nameof(SpGetLoansByBranchId);
        public const string SpGetLoanForEdit = nameof(SpGetLoanForEdit);
        public const string SpGetVSchedulesByPaymentDate = nameof(SpGetVSchedulesByPaymentDate);//spGetVSchedulesByPaymentDate
        public const string SpGetLateVSchedules = nameof(SpGetLateVSchedules);
        /// <summary>
        /// Employee Store Procedures
        /// </summary>
        public const string SpGetEmployeeByIdWithLoans = nameof(SpGetEmployeeByIdWithLoans);
        public const string SpGetLoansByEmployeeId = nameof(SpGetLoansByEmployeeId);
        public const string SpGetLoanRequestsByEmployeeId = nameof(SpGetLoanRequestsByEmployeeId);


    }
}
