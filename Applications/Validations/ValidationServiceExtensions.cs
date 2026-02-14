using Applications.Features.Activities.Command.CreateActivity;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Validations;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Currencies.Dto;
using Applications.Features.Customers.Commands.Customers.CreateCustomer;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Requests;
using Applications.Features.Customers.Validations;
using Applications.Features.Employees.Dto;
using Applications.Features.Employees.Requests;
using Applications.Features.Entities;
using Applications.Features.Guarantor.Command.CreateGuarantor;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.Identity.Roles.Requests;
using Applications.Features.Identity.Users;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Requests;
using Applications.Features.Settings.Requests;
using Applications.Features.Settings.Validations;
using Applications.Requests.Journals;
using Applications.Requests.Validations.Auths;
using Applications.Requests.Validations.Currencies;
using Applications.Requests.Validations.Employees;
using Applications.Requests.Validations.Loans;
using Applications.Requests.Validations.Transactions;
using Applications.Validations.Employees;
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
            //services.AddTransient<IValidator<EntityVendorCreate>, VendorValidator>();
            services.AddTransient<IValidator<CustomerDtoUpdate>, CustomerDtoUpdateValidator>();
            services.AddTransient<IValidator<LoanDtoCreate>, CreateLoanCommandValidation>();
            services.AddTransient<IValidator<LedgerCreateDto>, LedegerCreateDtoValidator>();
            services.AddTransient<IValidator<LedgerUpdateDto>, LedegerUpdateDtoValidator>();
            services.AddTransient<IValidator<ChangePasswordDto>, ChangedPasswordValidator>();
            services.AddTransient<IValidator<UserLoanSizeDtoCreate>, UserLoanSizeDtoCreateValidater>();
            services.AddTransient<IValidator<LoanRequestDtoCreate>, LoanRequestValidator>();
            services.AddTransient<IValidator<ExchangeDtoCreate>, CurrencyExchangeValidator>();
            services.AddTransient<IValidator<ActivityDtoCreate>, CustomerActivityDtoCreateValidator>();
            services.AddTransient<IValidator<CollateralSubTypeDtoCreate>, CollateralSubTypeValidator>();
            services.AddTransient<IValidator<ProfileDtoCreate>, ProfileValidator>();
            return services;
        }
    }
}
