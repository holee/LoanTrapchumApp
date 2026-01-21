
namespace Applications.Enumerations;

public enum TransactionTypes
{
    [System.ComponentModel.Description("Cash Deposit")]
    CD = 1,

    [System.ComponentModel.Description("Cash Withdrawal")]
    CW = 2,

    [System.ComponentModel.Description("Overdraft")]
    OD = 3,

    [System.ComponentModel.Description("Commission")]
    COMM = 4,

    [System.ComponentModel.Description("Cheque Deposit")]
    CHQD = 5,

    [System.ComponentModel.Description("Cheque Withdrawal")]
    CHQW = 6,

    [System.ComponentModel.Description("Account Transfer")]
    TRF = 7,

    [System.ComponentModel.Description("Saving Account Interest")]
    SAINT = 8,

    [System.ComponentModel.Description("Overdraft Interest")]
    ODINT = 9,

    [System.ComponentModel.Description("Inward Remittance")]
    INR = 10,

    [System.ComponentModel.Description("Outward Remittance")]
    OUR = 11,

    [System.ComponentModel.Description("Cashier's Order")]
    CO = 12,

    [System.ComponentModel.Description("Demand Draft")]
    DD = 13,

    [System.ComponentModel.Description("Service Charge")]
    SC = 14,

    [System.ComponentModel.Description("Fixed Deposit")]
    FD = 15,

    [System.ComponentModel.Description("Fixed Deposit Rollover")]
    FDRO = 16,

    [System.ComponentModel.Description("Fixed Deposit Interest")]
    FDINT = 17,

    [System.ComponentModel.Description("Fixed Deposit Withdrawn")]
    FDW = 18,

    [System.ComponentModel.Description("Loan Disbursement")]
    LD = 19,

    [System.ComponentModel.Description("Loan Principal Repayment")]
    LP = 20,

    [System.ComponentModel.Description("Loan Interest")]
    LINT = 21,

    [System.ComponentModel.Description("Penalizing Charge for late payment")]
    PC = 22,

    [System.ComponentModel.Description("Letter of Credit")]
    LC = 23,

    [System.ComponentModel.Description("Banker's Guarantee")]
    BG = 24,

    [System.ComponentModel.Description("Shipping Guarantee")]
    SG = 25,

    [System.ComponentModel.Description("Inward Bills")]
    IB = 26,

    [System.ComponentModel.Description("Reversal Entry")]
    REV = 27,

    [System.ComponentModel.Description("Correction Entry")]
    CORR = 28,

    [System.ComponentModel.Description("Accrued Interest")]
    ACCINT = 29,

    [System.ComponentModel.Description("Outward Cheque Clearance")]
    OUTCLR = 30,

    [System.ComponentModel.Description("Inward Cheque Clearance")]
    INCLR = 31,

    [System.ComponentModel.Description("Officer Tasks")]
    OFFTSK = 32,

    [System.ComponentModel.Description("Journal Entries")]
    JORNL = 33,

    [System.ComponentModel.Description("Savings Account Tax Payment")]
    SAVTAX = 34,

    [System.ComponentModel.Description("Cheque Return")]
    CHQR = 35,

    [System.ComponentModel.Description("Cheque Clear")]
    CHQC = 36,

    [System.ComponentModel.Description("Balance Brought Forward")]
    BF = 37,

    [System.ComponentModel.Description("Customer Initial Balance")]
    INI = 38,

    [System.ComponentModel.Description("Interest Adjustment")]
    INTADJ = 39,

    [System.ComponentModel.Description("Fixed Deposit Withholding tax")]
    FDTAX = 40,

    [System.ComponentModel.Description("Fixed Deposit With Cheque")]
    FDWC = 50,

    [System.ComponentModel.Description("Yearly Close")]
    YAEREND = 51,

    [System.ComponentModel.Description("Daily Depreciation")]
    DDPC = 52,

    [System.ComponentModel.Description("Loan Transfer")]
    LTRF = 53,

    [System.ComponentModel.Description("Accumulate Gain/Loss daily")]
    AGL = 54,

    [System.ComponentModel.Description("Daily close capital ledger")]
    DCL = 55,

    [System.ComponentModel.Description("Clear Accrued Interest to Due from NBC")]
    CA_NBC = 56,

    [System.ComponentModel.Description("Loan Add Facility")]
    LAF = 57,

    [System.ComponentModel.Description("Loan Reduce Facility")]
    LRF = 58,

    [System.ComponentModel.Description("Loan Transfer Accrued Interest")]
    LTINT = 59,

    [System.ComponentModel.Description("Loan Delete")]
    LDEL = 60,

    [System.ComponentModel.Description("Close Saving Account")]
    CSA = 61,

    [System.ComponentModel.Description("Dormant Account Charge")]
    DAC = 62,

    [System.ComponentModel.Description("Cheque Certify")]
    CHQCTF = 63,

    [System.ComponentModel.Description("Cheque Certify Settlement")]
    CHQCSM = 64,

    [System.ComponentModel.Description("Letter of Credit Acceptance")]
    LCA = 65,

    [System.ComponentModel.Description("Letter of Credit Settlement")]
    LCS = 66,

    [System.ComponentModel.Description("Currency Foreign Exchange")]
    CFX = 67,

    [System.ComponentModel.Description("Revalue Gain/Loss daily")]
    RGL = 68,

    [System.ComponentModel.Description("Open Branch")]
    OP = 69,

    [System.ComponentModel.Description("Loan Transfer Interest Income")]
    LTII = 70,

    [System.ComponentModel.Description("Provision For Loan Outstanding")]
    PLP = 71,

    [System.ComponentModel.Description("Reverse Provision")]
    RP = 72,

    [System.ComponentModel.Description("Reverse Loan Disbursement")]
    RLD = 73,

    [System.ComponentModel.Description("Reverse Loan Payment")]
    RLP = 74,

    [System.ComponentModel.Description("Clear AIR and Interest Income on paid off loan")]
    CAI = 75,

    [System.ComponentModel.Description("Write off Loan")]
    WL = 76,

    [System.ComponentModel.Description("Amount Added to Principal of Restructured Loan")]
    AAPRL = 77,

    [System.ComponentModel.Description("Cash Vault")]
    CSV = 78
}