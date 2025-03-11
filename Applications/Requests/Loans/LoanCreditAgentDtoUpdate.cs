namespace Applications.Requests.Loans
{
    public class LoanCreditAgentDtoUpdate: LoanCreditAgentDtoCreate
    {
        public int LoanCreditAgentId { get; set; }
        public long EmployeeRefId { get; set; } 
        public bool IsRemove { get; set; }
        public int EditSeq { get; set; } = 0;

    }
}
