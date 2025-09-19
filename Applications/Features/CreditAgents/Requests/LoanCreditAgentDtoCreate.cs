namespace Applications.Features.CreditAgents.Requests
{
    public class LoanCreditAgentDtoCreate 
    {
        public int? CreditAssistant { get; set; }
        public int? SubManager { get; set; }
        public int? Manager { get; set; }
        public int? SubBranch { get; set; }
        public int? HeadOffice { get; set; }
    }
}
