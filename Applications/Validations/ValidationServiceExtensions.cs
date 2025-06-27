using Applications.Requests.Identity;
using Applications.Requests.Journals;
using Applications.Requests.Lists.Collaterals;
using Applications.Requests.Lists.Currency;
using Applications.Requests.Lists.Customers;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Requests.Loans.Disbursements;
using Applications.Requests.Loans.LoanRequests;
using Applications.Requests.Settings;
using Applications.Requests.Validations.Auths;
using Applications.Requests.Validations.Currencies;
using Applications.Requests.Validations.Employees;
using Applications.Requests.Validations.Loans;
using Applications.Requests.Validations.Transactions;
using Applications.Validations.Branches;
using Applications.Validations.Customers;
using Applications.Validations.Employees;
using Applications.Validations.NameEntities;
using Domains.Models.Identity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Applications.Validations
{
    public static class ValidationServiceExtensions
    {
        public static IServiceCollection AddValidationServiceInjections(this IServiceCollection services)
        {

            services.AddTransient<IValidator<BranchDtoCreate>, BranchDtoCreateValidator>();
            services.AddTransient<IValidator<BranchDtoUpdate>, BranchDtoUpdateValidator>();
            services.AddTransient<IValidator<RoleCreateDto>, RoleCreateDtoValidator>();
            services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddTransient<IValidator<TokenRequest>, TokenRequestValidator>();
            services.AddTransient<IValidator<EntityNameDtoCreate>, EntityNameDtoCreateValidator>();
            services.AddTransient<IValidator<EntityGuarantorCreate>, EntityGuarantorCreateValidator>();
            services.AddTransient<IValidator<EntityEmployeeCreate>, EntityEmployeeCreateValidator>();
            services.AddTransient<IValidator<EmployeeDtoUpdate>, EmployeeDtoUpdateValidator>();
            services.AddTransient<IValidator<EntityCustomerCreate>, CustomerDtoCreateValidator>();
            services.AddTransient<IValidator<EntityVendorCreate>, VendorValidator>();
            services.AddTransient<IValidator<CustomerDtoUpdate>, CustomerDtoUpdateValidator>();
            services.AddTransient<IValidator<LoanDtoCreate>, LoanCreateDtoValidator>();
            services.AddTransient<IValidator<LedgerCreateDto>, LedegerCreateDtoValidator>();
            services.AddTransient<IValidator<LedgerUpdateDto>, LedegerUpdateDtoValidator>();
            services.AddTransient<IValidator<ChangePasswordDto>, ChangedPasswordValidator>();
            services.AddTransient<IValidator<UserLoanSizeDtoCreate>, UserLoanSizeDtoCreateValidater>();
            services.AddTransient<IValidator<LoanRequestDtoCreate>, LoanRequestValidator>();
            services.AddTransient<IValidator<ExchangeDtoCreate>, CurrencyExchangeValidator>();
            services.AddTransient<IValidator<CustomerActivityDtoCreate>, CustomerActivityDtoCreateValidator>();
            services.AddTransient<IValidator<CollateralSubTypeDtoCreate>, CollateralSubTypeValidator>();
            services.AddTransient<IValidator<ProfileDtoCreate>, ProfileValidator>();
            return services;
        }
    }
}
