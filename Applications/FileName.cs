namespace Applications;

using System;

public sealed class LoanInformation
{
    public string? CustomerName { get; set; }          // Example: សេង ម៉ៅ
    public string? CustomerCode { get; set; }          // 06S1711
    public string? AccountNumber { get; set; }         // 333-001-005744-1
    public DateTime? LoanDate { get; set; }            // 29-05-2026
    public int? LoanTermMonths { get; set; }           // 6
    public string? Address { get; set; }               // Cambodia address
    public decimal? LoanAmount { get; set; }           // 2,000,000
    public DateTime? MaturityDate { get; set; }        // 07-12-2026
    public string? GuarantorName { get; set; }         // សេង ម៉េងស្រ៊ុន
    public string? AgentName { get; set; }         // Microbest / TrapChum Tbeng 
    public string? PhoneNumber1 { get; set; }          // 060 30 64 23
    public decimal? InterestRate { get; set; }         // 1.10 (%)
    public string? PhoneNumber2 { get; set; }          // 081 227 942
    public int? Cycle { get; set; }                    // 1
}