namespace Applications.Requests.Loans.CreditAgents
{
    public class LoanCreditAgentDtoActivate
    {
        public Guid? EmployeeRefId { get; set; } 
        public int EditSeq { get; set; }
        public bool IsRemove { get; set; }  
    }
}
