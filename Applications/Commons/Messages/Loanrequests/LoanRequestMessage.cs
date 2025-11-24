namespace Applications.Commons.Messages.Loanrequests
{
    public static class LoanRequestMessage
    {
        public const string REQUEST_NOT_FOUND = "Loan request not found.";
        public const string REQUEST_ALREADY_EXISTS = "Loan request already exists.";
        public const string CUSTOMER_HAS_ACTIVE_LOAN = "Customer has active loan.";
        public const string REQUEST_APPROVED = "Loan request approved successfully.";
        public const string REQUEST_DENIED = "Loan request denied successfully.";
        public const string INVALID_REQUEST_AMOUNT = "Invalid request amount.";
        public const string INVALID_INTEREST_RATE = "Invalid interest rate.";
        public const string INVALID_INSTALLMENTS = "Invalid number of installments.";
        public const string GUARANTOR_REMOVED = "Guarantor is removed.";
        public const string NO_GUARANTOR = "There is no any guarantor is selected."; 
        public const string GUARANTOR_ADDED = "Guarantor is added.";
        public const string COLLATERAL_ADDED = "Collateral is added.";
        public const string COLLATERAL_REMOVED = "Collateral is removed.";
        public const string REQUEST_CREATED = "Loan request created successfully.";
        public const string REQUEST_UPDATED = "Loan request updated successfully.";
        public const string REQUEST_DELETED = "Loan request deleted successfully.";
        public const string REQUEST_CANNOT_BE_DELETED = "Loan request cannot be deleted.";
        public const string REQUEST_CANNOT_BE_UPDATED = "Loan request cannot be updated.";
        public const string REQUEST_CANNOT_BE_APPROVED = "Loan request cannot be approved.";
        public const string REQUEST_CANNOT_BE_DENIED = "Loan request cannot be denied.";
        public const string REQUEST_DISBURSED = "Loan request to be disbursed.";
        public const string REQUEST_CANNOT_BE_DISBURSED = "Loan request cannot be disbursed.";
        public const string INVALID_ADMIN_FEE_RATE = "Invalid admin fee rate.";
        public const string REQUEST_ALREADY_DISBURSED = "Loan request already disbursed.";
        public const string REQUEST_NOT_APPROVED = "Loan request is not approved.";
        public const string REQUEST_IN_USED = "cannot remove this request due to it is in used.";
    }
}
