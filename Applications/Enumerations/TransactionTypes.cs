using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Applications.Enumerations;

public enum TransactionTypes
{
    [Description("Cash Deposit")]
    CD = 1,

    [Description("Cash Withdrawal")]
    CW = 2,

    [Description("Overdraft")]
    OD = 3,

    [Description("Commission")]
    COMM = 4,

    [Description("Cheque Deposit")]
    CHQD = 5,

    [Description("Cheque Withdrawal")]
    CHQW = 6,

    [Description("Account Transfer")]
    TRF = 7,

    [Description("Saving Account Interest")]
    SAINT = 8,

    [Description("Overdraft Interest")]
    ODINT = 9,

    [Description("Inward Remittance")]
    INR = 10,

    [Description("Outward Remittance")]
    OUR = 11,

    [Description("Cashier's Order")]
    CO = 12,

    [Description("Demand Draft")]
    DD = 13,

    [Description("Service Charge")]
    SC = 14,

    [Description("Fixed Deposit")]
    FD = 15,

    [Description("Fixed Deposit Rollover")]
    FDRO = 16,

    [Description("Fixed Deposit Interest")]
    FDINT = 17,

    [Description("Fixed Deposit Withdrawn")]
    FDW = 18,

    [Description("Loan Disbursement")]
    LD = 19,

    [Description("Loan Principal Repayment")]
    LP = 20,

    [Description("Loan Interest")]
    LINT = 21,

    [Description("Penalizing Charge for late payment")]
    PC = 22,

    [Description("Letter of Credit")]
    LC = 23,

    [Description("Banker's Guarantee")]
    BG = 24,

    [Description("Shipping Guarantee")]
    SG = 25,

    [Description("Inward Bills")]
    IB = 26,

    [Description("Reversal Entry")]
    REV = 27,

    [Description("Correction Entry")]
    CORR = 28,

    [Description("Accrued Interest")]
    ACCINT = 29,

    [Description("Outward Cheque Clearance")]
    OUTCLR = 30,

    [Description("Inward Cheque Clearance")]
    INCLR = 31,

    [Description("Officer Tasks")]
    OFFTSK = 32,

    [Description("Journal Entries")]
    JORNL = 33,

    [Description("Savings Account Tax Payment")]
    SAVTAX = 34,

    [Description("Cheque Return")]
    CHQR = 35,

    [Description("Cheque Clear")]
    CHQC = 36,

    [Description("Balance Brought Forward")]
    BF = 37,

    [Description("Customer Initial Balance")]
    INI = 38,

    [Description("Interest Adjustment")]
    INTADJ = 39,

    [Description("Fixed Deposit Withholding tax")]
    FDTAX = 40,

    [Description("Fixed Deposit With Cheque")]
    FDWC = 50,

    [Description("Yearly Close")]
    YAEREND = 51,

    [Description("Daily Depreciation")]
    DDPC = 52,

    [Description("Loan Transfer")]
    LTRF = 53,

    [Description("Accumulate Gain/Loss daily")]
    AGL = 54,

    [Description("Daily close capital ledger")]
    DCL = 55,

    [Description("Clear Accrued Interest to Due from NBC")]
    CA_NBC = 56,

    [Description("Loan Add Facility")]
    LAF = 57,

    [Description("Loan Reduce Facility")]
    LRF = 58,

    [Description("Loan Transfer Accrued Interest")]
    LTINT = 59,

    [Description("Loan Delete")]
    LDEL = 60,

    [Description("Close Saving Account")]
    CSA = 61,

    [Description("Dormant Account Charge")]
    DAC = 62,

    [Description("Cheque Certify")]
    CHQCTF = 63,

    [Description("Cheque Certify Settlement")]
    CHQCSM = 64,

    [Description("Letter of Credit Acceptance")]
    LCA = 65,

    [Description("Letter of Credit Settlement")]
    LCS = 66,

    [Description("Currency Foreign Exchange")]
    CFX = 67,

    [Description("Revalue Gain/Loss daily")]
    RGL = 68,

    [Description("Open Branch")]
    OP = 69,

    [Description("Loan Transfer Interest Income")]
    LTII = 70,

    [Description("Provision For Loan Outstanding")]
    PLP = 71,

    [Description("Reverse Provision")]
    RP = 72,

    [Description("Reverse Loan Disbursement")]
    RLD = 73,

    [Description("Reverse Loan Payment")]
    RLP = 74,

    [Description("Clear AIR and Interest Income on paid off loan")]
    CAI = 75,

    [Description("Write off Loan")]
    WL = 76,

    [Description("Amount Added to Principal of Restructured Loan")]
    AAPRL = 77,

    [Description("Cash Vault")]
    CSV = 78
}