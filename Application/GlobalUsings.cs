// ═══════════════════════════════════════════════════════════════════
// Framework & Third-Party
// ═══════════════════════════════════════════════════════════════════
global using FluentValidation;
global using Microsoft.Extensions.DependencyInjection;

// ═══════════════════════════════════════════════════════════════════
// Application — Core / Base
// ═══════════════════════════════════════════════════════════════════
global using Application.Bases;
global using Application.Commons;
global using Application.Responses;
global using Application.Responses.Paginations;
global using Application.Utils;

// ═══════════════════════════════════════════════════════════════════
// Application — Services (UI-consumed façades)
// ═══════════════════════════════════════════════════════════════════
global using Application.Services.Files;
global using Application.Services.Identity;
global using Application.Services.Lists;
global using Application.Services.Loans;
global using Application.Services.Reports;
global using Application.Services.Transactions.Branches;
global using Application.Services.Transactions.Currencies;
global using Application.Services.Transactions.Journals;

// ═══════════════════════════════════════════════════════════════════
// Application — Interfaces
// ═══════════════════════════════════════════════════════════════════
global using Application.Interfaces.Loans;

// ═══════════════════════════════════════════════════════════════════
// Application — Responses / ViewModels
// ═══════════════════════════════════════════════════════════════════
global using Application.Responses.ViewModels.Auths;
global using Application.Responses.ViewModels.Customers;
global using Application.Responses.ViewModels.Employees;
global using Application.Responses.ViewModels.Loans.Requests;
global using Application.Responses.ViewModels.Loans.Schedules;
global using Application.Responses.ViewModels.OtherNames;

// ═══════════════════════════════════════════════════════════════════
// Features — Activities
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Activities.Command.CreateActivity;

// ═══════════════════════════════════════════════════════════════════
// Features — Additional Informations (Occupations, Nationalities, Salutations)
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.AdditionalInformations.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Addresses
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Addresses.Dto;
global using Application.Features.Addresses.Interfaces;

// ═══════════════════════════════════════════════════════════════════
// Features — Banks
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Banks;

// ═══════════════════════════════════════════════════════════════════
// Features — Branches
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Branches.Dtos;
global using Application.Features.Branches.Queries.GetBranchAsPaging;
global using Application.Features.Branches.Queries.GetBranchsByUser;

// ═══════════════════════════════════════════════════════════════════
// Features — Cash Managements
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.CashManagements.Interfaces;
global using Application.Features.CashManagements.Responses;

// ═══════════════════════════════════════════════════════════════════
// Features — Collaterals
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Collaterals.Dtos;
global using Application.Features.Collaterals.Queries.GetCollateralDetails;
global using Application.Features.Collaterals.Queries.GetCollateralLineById;
global using Application.Features.Collaterals.Queries.GetCollateralLineDetails;

// ═══════════════════════════════════════════════════════════════════
// Features — Currencies
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Currencies;
global using Application.Features.Currencies.Command.AddCurrencyNote;
global using Application.Features.Currencies.Dto;
global using Application.Features.Currencies.Queries.GetCurrencyDetails;
global using Application.Features.Currencies.Responses;

// ═══════════════════════════════════════════════════════════════════
// Features — Customers
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Customers.Commands.CreateCustomer;
global using Application.Features.Customers.Commands.EditCustomer;
global using Application.Features.Customers.Dto;
global using Application.Features.Customers.Interfaces;
global using Application.Features.Customers.Response;
global using Application.Features.Customers.UseCases;

// ═══════════════════════════════════════════════════════════════════
// Features — Dashboard
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Dashbaord.Accountants.Dtos;
global using Application.Features.Dashbaord.CreditOfficers.Dtos;
global using Application.Features.Dashbaord.Managers.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Departments
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Departments.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Documents
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Documents.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Employees
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Employees.Commands.EditEmployee;
global using Application.Features.Employees.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Expenses
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Expenses.CreateExpenseCommands;
global using Application.Features.Expenses.CreateExpensesCommands;
global using Application.Features.Expenses.Dto;
global using Application.Features.Expenses.EditExpenseCommands;
global using Application.Features.Expenses.EditExpensesCommands;
global using Application.Features.Expenses.Queries.GetExpenseByCustomerId;

// ═══════════════════════════════════════════════════════════════════
// Features — Guarantor
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Guarantor.Dtos;

// ═══════════════════════════════════════════════════════════════════
// Features — Holidays
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Holidays.Command.CreateHolidays;
global using Application.Features.Holidays.Queries.GetActiveHolidays;

// ═══════════════════════════════════════════════════════════════════
// Features — Identity
// ═══════════════════════════════════════════════════════════════════

// ═══════════════════════════════════════════════════════════════════
// Features — Incomes
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Incomes.Command.CreateIncome;
global using Application.Features.Incomes.Command.CreateIncomes;
global using Application.Features.Incomes.Command.EditIncome;
global using Application.Features.Incomes.Command.EditIncomeExpenseSource;
global using Application.Features.Incomes.Command.GetEditIncomeExpenseSource;
global using Application.Features.Incomes.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Journals
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Journals.Dtos;

// ═══════════════════════════════════════════════════════════════════
// Features — Ledgers
// ═══════════════════════════════════════════════════════════════════

// ═══════════════════════════════════════════════════════════════════
// Features — Loan Payments
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.LoanPayments.Command.RecievePayment;
global using Application.Features.LoanPayments.Dtos;
global using Application.Features.LoanPayments.Interfaces;
global using Application.Features.LoanPayments.UseCases;

// ═══════════════════════════════════════════════════════════════════
// Features — Loan Purposes
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.LoanPurposes;

// ═══════════════════════════════════════════════════════════════════
// Features — Loan Requests
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.LoanRequests.Command.CreateLoanRequest;
global using Application.Features.LoanRequests.Command.SaveEditLoanRequest;
global using Application.Features.LoanRequests.Command.SaveLoanRequest;

// ═══════════════════════════════════════════════════════════════════
// Features — Loans
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Loans.Command.SaveLoanDisbursemenent;
global using Application.Features.Loans.Dtos;
global using Application.Features.Loans.Dtos.Responses;
global using Application.Features.Loans.Services;

// ═══════════════════════════════════════════════════════════════════
// Features — Notifications
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Notifications.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Notes
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Notes.Command.CreateNote;

// ═══════════════════════════════════════════════════════════════════
// Features — OtherNames
// ═══════════════════════════════════════════════════════════════════

// ═══════════════════════════════════════════════════════════════════
// Features — Periods
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Periods.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Schedules
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Schedules.Dto;

// ═══════════════════════════════════════════════════════════════════
// Features — Settings
// ═══════════════════════════════════════════════════════════════════
global using Application.Features.Settings.Dto;
global using Application.Features.Settings.UseCases;

// ═══════════════════════════════════════════════════════════════════
// Domains
// ═══════════════════════════════════════════════════════════════════
global using Domains.Models.Currencies;
global using Domains.Models.Journals;
global using Domains.Models.Lists;