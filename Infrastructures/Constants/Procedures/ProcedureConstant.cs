namespace Infrastructures.Constants.Procedures
{
    public static class ProcedureConstant
    {
        public const string GetCustomerDetailsById = "spGetCustomerDetailsById";
        public const string GetCustomerDetailById = "spGetCustomerDetailById";
        public const string GetLoanById = "spGetLoanById";
        public const string GetScheduleById = "spGetScheduleById";
        public const string GetCurrencyNotes = "getCurrencyNotes";
        public const string GetCurrencyNotesByBranchId = "GetCurrencyNotesByBranchId";
        public const string SpGetSchedulesByLoanId = nameof(SpGetSchedulesByLoanId);
        public const string SpGetLoansByBranchId = nameof(SpGetLoansByBranchId);
    }
}
