// ═══════════════════════════════════════════════════════════════════
// Framework & Third-Party
// ═══════════════════════════════════════════════════════════════════
global using FluentValidation;
global using Microsoft.Extensions.DependencyInjection;

// ═══════════════════════════════════════════════════════════════════
// Applications — Core / Base
// ═══════════════════════════════════════════════════════════════════
global using Applications.Bases;
global using Applications.Commons;
global using Applications.Responses;
global using Applications.Responses.Paginations;
global using Applications.Utils;

// ═══════════════════════════════════════════════════════════════════
// Applications — Services (UI-consumed façades)
// ═══════════════════════════════════════════════════════════════════
global using Applications.Services.Files;
global using Applications.Services.Identity;
global using Applications.Services.Lists;
global using Applications.Services.Loans;
global using Applications.Services.Reports;
global using Applications.Services.Transactions.Branches;
global using Applications.Services.Transactions.Currencies;
global using Applications.Services.Transactions.Journals;

// ═══════════════════════════════════════════════════════════════════
// Applications — Interfaces
// ═══════════════════════════════════════════════════════════════════
global using Applications.Interfaces.Loans;

// ═══════════════════════════════════════════════════════════════════
// Applications — Responses / ViewModels
// ═══════════════════════════════════════════════════════════════════
global using Applications.Responses.ViewModels.Auths;
global using Applications.Responses.ViewModels.Customers;
global using Applications.Responses.ViewModels.Employees;
global using Applications.Responses.ViewModels.Loans.Requests;
global using Applications.Responses.ViewModels.Loans.Schedules;
global using Applications.Responses.ViewModels.OtherNames;

// ═══════════════════════════════════════════════════════════════════
// Features — Activities
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Activities.Command.CreateActivity;

// ═══════════════════════════════════════════════════════════════════
// Features — Additional Informations (Occupations, Nationalities, Salutations)
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.AdditionalInformations.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Addresses
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Addresses.Dto;
global using Applications.Features.Addresses.Interfaces;

// ═══════════════════════════════════════════════════════════════════
// Features — Banks
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Banks;

// ═══════════════════════════════════════════════════════════════════
// Features — Branches
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Branches.Dtos;
global using Applications.Features.Branches.Queries.GetBranchAsPaging;
global using Applications.Features.Branches.Queries.GetBranchsByUser;

// ═══════════════════════════════════════════════════════════════════
// Features — Cash Managements
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.CashManagements.Interfaces;
global using Applications.Features.CashManagements.Responses;

// ═══════════════════════════════════════════════════════════════════
// Features — Collaterals
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Collaterals.Dtos;
global using Applications.Features.Collaterals.Queries.GetCollateralDetails;
global using Applications.Features.Collaterals.Queries.GetCollateralLineById;
global using Applications.Features.Collaterals.Queries.GetCollateralLineDetails;

// ═══════════════════════════════════════════════════════════════════
// Features — Currencies
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Currencies;
global using Applications.Features.Currencies.Command.AddCurrencyNote;
global using Applications.Features.Currencies.Dto;
global using Applications.Features.Currencies.Queries.GetCurrencyDetails;
global using Applications.Features.Currencies.Responses;

// ═══════════════════════════════════════════════════════════════════
// Features — Customers
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Customers.Commands.CreateCustomer;
global using Applications.Features.Customers.Commands.EditCustomer;
global using Applications.Features.Customers.Dto;
global using Applications.Features.Customers.Interfaces;
global using Applications.Features.Customers.Response;
global using Applications.Features.Customers.UseCases;

// ═══════════════════════════════════════════════════════════════════
// Features — Dashboard
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Dashbaord.Accountants.Dtos;
global using Applications.Features.Dashbaord.CreditOfficers.Dtos;
global using Applications.Features.Dashbaord.Managers.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Departments
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Departments.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Documents
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Documents.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Employees
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Employees.Commands.EditEmployee;
global using Applications.Features.Employees.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Expenses
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Expenses.CreateExpenseCommands;
global using Applications.Features.Expenses.CreateExpensesCommands;
global using Applications.Features.Expenses.Dto;
global using Applications.Features.Expenses.EditExpenseCommands;
global using Applications.Features.Expenses.EditExpensesCommands;
global using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;

// ═══════════════════════════════════════════════════════════════════
// Features — Guarantor
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Guarantor.Dtos;

// ═══════════════════════════════════════════════════════════════════
// Features — Holidays
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Holidays.Command.CreateHolidays;
global using Applications.Features.Holidays.Queries.GetActiveHolidays;

// ═══════════════════════════════════════════════════════════════════
// Features — Identity
// ═══════════════════════════════════════════════════════════════════

// ═══════════════════════════════════════════════════════════════════
// Features — Incomes
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Incomes.Command.CreateIncome;
global using Applications.Features.Incomes.Command.CreateIncomes;
global using Applications.Features.Incomes.Command.EditIncome;
global using Applications.Features.Incomes.Command.EditIncomeExpenseSource;
global using Applications.Features.Incomes.Command.GetEditIncomeExpenseSource;
global using Applications.Features.Incomes.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Journals
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Journals.Dtos;

// ═══════════════════════════════════════════════════════════════════
// Features — Ledgers
// ═══════════════════════════════════════════════════════════════════

// ═══════════════════════════════════════════════════════════════════
// Features — Loan Payments
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.LoanPayments.Command.RecievePayment;
global using Applications.Features.LoanPayments.Dtos;
global using Applications.Features.LoanPayments.Interfaces;
global using Applications.Features.LoanPayments.UseCases;

// ═══════════════════════════════════════════════════════════════════
// Features — Loan Purposes
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.LoanPurposes;

// ═══════════════════════════════════════════════════════════════════
// Features — Loan Requests
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.LoanRequests.Command.CreateLoanRequest;
global using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
global using Applications.Features.LoanRequests.Command.SaveLoanRequest;

// ═══════════════════════════════════════════════════════════════════
// Features — Loans
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Loans.Command.SaveLoanDisbursemenent;
global using Applications.Features.Loans.Dtos;
global using Applications.Features.Loans.Dtos.Responses;
global using Applications.Features.Loans.Services;

// ═══════════════════════════════════════════════════════════════════
// Features — Notifications
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Notifications.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Notes
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Notes.Command.CreateNote;

// ═══════════════════════════════════════════════════════════════════
// Features — OtherNames
// ═══════════════════════════════════════════════════════════════════

// ═══════════════════════════════════════════════════════════════════
// Features — Periods
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Periods.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Schedules
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Schedules.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Settings
// ═══════════════════════════════════════════════════════════════════
global using Applications.Features.Settings.Dto;
global using Applications.Features.Settings.UseCases;

// ═══════════════════════════════════════════════════════════════════
// Domains
// ═══════════════════════════════════════════════════════════════════
global using Domains.Models.Currencies;
global using Domains.Models.Journals;
global using Domains.Models.Lists;