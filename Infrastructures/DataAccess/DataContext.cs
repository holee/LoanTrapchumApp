using Applications.Features.Branches.Dtos;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Currencies.Responses;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Response;
using Applications.Features.Departments.Dto;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Responses.ViewModels.Addresses;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Responses.ViewModels.Settings;
using Domains.Models;
using Domains.Models.Addresses;
using Domains.Models.Audits;
using Domains.Models.Bases;
using Domains.Models.Collaterals;
using Domains.Models.Currencies;
using Domains.Models.Journals;
using Domains.Models.Lists;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Domains.Models.Transaction;
using Infrastructures.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Infrastructures.DataAccess
{
    public sealed class DataContext : DbContext
    {
        private readonly AuditingSaveChangesInterceptor _auditInterceptor;
        public DataContext(DbContextOptions<DataContext> options, AuditingSaveChangesInterceptor auditInterceptor) : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var property in builder.Model.GetEntityTypes()
               .SelectMany(t => t.GetProperties())
               .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,3)");
            }

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }

        public IDbTransaction StartTransaction()
        {
            var transaction = Database.BeginTransaction();

            return transaction.GetDbTransaction();
        }

        public override int SaveChanges()
        {
            ProcessSaveChangedAsync();

            return base.SaveChanges();
        }

        private void ProcessSaveChangedAsync()
        {
            foreach (var item in ChangeTracker.Entries()
                                .Where(e => e.Entity is BaseAddress && e.State == EntityState.Added))
            {
                BaseAddress baseEntry = (BaseAddress)item.Entity;
                baseEntry.CreatedDt = DateTimeOffset.UtcNow;
                baseEntry.ModifiedDt = DateTimeOffset.UtcNow;
            }

            foreach (var item in ChangeTracker.Entries()
                               .Where(e => e.Entity is BaseAddress && e.State == EntityState.Modified))
            {
                var baseEntries = (BaseAddress)item.Entity;
                baseEntries.ModifiedDt = DateTimeOffset.UtcNow;
                baseEntries.EditSeq += 1;
                item.Property(nameof(BaseAddress.CreatedDt)).IsModified = false;

            }
            //for entity inherti from base entity

            foreach (var item in ChangeTracker.Entries()
                                .Where(e => e.Entity is BaseEntity && e.State == EntityState.Added))
            {
                BaseEntity baseEntry = (BaseEntity)item.Entity;
                baseEntry.CreatedDt = DateTime.Now;
                baseEntry.ModifiedDt = DateTime.Now;
            }

            foreach (var item in ChangeTracker.Entries()
                               .Where(e => e.Entity is BaseEntity && e.State == EntityState.Modified))
            {
                var baseEntries = (BaseEntity)item.Entity;
                baseEntries.ModifiedDt = DateTimeOffset.UtcNow;
                baseEntries.EditSeq += 1;
                item.Property(nameof(BaseEntity.CreatedDt)).IsModified = false;

            }

        }


        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{

        //    ProcessSaveChangedAsync();

        //    return await base.SaveChangesAsync(cancellationToken);

        //}


        public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();
        public DbSet<Bank> Banks => Set<Bank>();
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<CurrencyCounter> CurrencyCounters => Set<CurrencyCounter>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Collateral> Collaterals => Set<Collateral>();
        public DbSet<CollateralLink> CollateralsLinks => Set<CollateralLink>();
        public DbSet<CollateralLine> CollateralLines => Set<CollateralLine>();
        public DbSet<CollateralEvaluations> CollateralEvaluations => Set<CollateralEvaluations>();
        public DbSet<CollateralType> CollateralTypes => Set<CollateralType>();
        public DbSet<CollateralSubType> CollateralSubTypes => Set<CollateralSubType>();
        public DbSet<LoanPropertyType> LoanPropertyType => Set<LoanPropertyType>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<UserLoanSize> UserLoanSizes => Set<UserLoanSize>();
        public DbSet<Salutation> Salutations => Set<Salutation>();
        public DbSet<Reciept> Reciepts => Set<Reciept>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Vendor> Vendors => Set<Vendor>();
        public DbSet<LoanCreditAgent> LoanCreditAgents => Set<LoanCreditAgent>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerExpense> CustomerExpenses => Set<CustomerExpense>();
        public DbSet<CustomerIncome> CustomerIncomes => Set<CustomerIncome>();
        public DbSet<IncomeExpense> IncomeExpenses => Set<IncomeExpense>();
        public DbSet<EntityNames> EntityNames => Set<EntityNames>();
        public DbSet<Occupation> Occupations => Set<Occupation>();
        public DbSet<LtPeriod> LtPeriods => Set<LtPeriod>();
        public DbSet<LtPurpose> LtPurposes => Set<LtPurpose>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<Commune> Communes => Set<Commune>();
        public DbSet<CustomerNote> CustomerNote => Set<CustomerNote>();
        public DbSet<CustomerActivity> CustomerActivites => Set<CustomerActivity>();
        public DbSet<Village> Villages => Set<Village>();
        public DbSet<Nationality> Nationalities => Set<Nationality>();
        public DbSet<Ledger> Ledgers => Set<Ledger>();
        public DbSet<LedgerType> LedgerTypes => Set<LedgerType>();
        public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
        public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
        public DbSet<JournalType> JournalTypes => Set<JournalType>();
        public DbSet<LoanDisbursment> LoanDisbursments => Set<LoanDisbursment>();
        public DbSet<LoanPayment> LoanRepayments => Set<LoanPayment>();
        public DbSet<LoanPaymentLine> LoanRepaymentLine => Set<LoanPaymentLine>();
        public DbSet<LoanSchedule> LoanSchedules => Set<LoanSchedule>();
        public DbSet<LoanStatus> LoanStatuses => Set<LoanStatus>();
        public DbSet<LoanCategory> LoanCategories => Set<LoanCategory>();
        public DbSet<LoanProduct> LoanProducts => Set<LoanProduct>();
        public DbSet<LoanClarifyOfStaff> LoanClarifyOfStaff => Set<LoanClarifyOfStaff>();
        public DbSet<LoanRepaymentMode> LoanRepaymentModes => Set<LoanRepaymentMode>();
        public DbSet<LoanInstallmentType> LoanInstallmentTypes => Set<LoanInstallmentType>();
        public DbSet<LoanTransaction> LoanTransaction => Set<LoanTransaction>();
        public DbSet<LoanTransactionType> TransactionTypes => Set<LoanTransactionType>();
        public DbSet<LoanScheduleStatus> LoanScheduleStatuses => Set<LoanScheduleStatus>();
        public DbSet<LoanPaymentLocation> LoanPaymentLocations => Set<LoanPaymentLocation>();
        public DbSet<OpenBranch> OpenBranches => Set<OpenBranch>();
        public DbSet<CashVault> CashVaults => Set<CashVault>();
        public DbSet<CashManagement> CashManagements => Set<CashManagement>();
        public DbSet<CashManagementDetail> CashManagementDetails => Set<CashManagementDetail>();
        public DbSet<CashStatus> CashStatuses => Set<CashStatus>();
        public DbSet<ForeignExchange> ForeignExchanges => Set<ForeignExchange>();
        public DbSet<LoanTermType> LoanTerms => Set<LoanTermType>();
        public DbSet<LoanTypeStatus> LoanSubTypeStatuses => Set<LoanTypeStatus>();
        public DbSet<LoanSubTypeStatus> LoanSubTypeNoAcc => Set<LoanSubTypeStatus>();
        public DbSet<LoanGuarantorLink> Guarantors => Set<LoanGuarantorLink>();
        public DbSet<CoPawnCustomer> CoPawnCustomers => Set<CoPawnCustomer>();
        public DbSet<AppStatus> AppStatuses => Set<AppStatus>();
        public DbSet<CurrencySpotRate> SpotRates => Set<CurrencySpotRate>();
        public DbSet<ActivitiesStatus> ActivitiesStatuses => Set<ActivitiesStatus>();
        public DbSet<BankOther> BankOthers => Set<BankOther>();
        public DbSet<DayName> DayNames => Set<DayName>();
        public DbSet<Holiday> Holidays => Set<Holiday>();
        public DbSet<MonthName> MonthNames => Set<MonthName>();
        public DbSet<OtherType> OtherTypes => Set<OtherType>();
        public DbSet<MarritalStatus> MarritalStatuses => Set<MarritalStatus>();
        public DbSet<LoanDisbursment> LoanDisbursementRequests => Set<LoanDisbursment>();
        public DbSet<LoanRequest> LoanRequests => Set<LoanRequest>();
        public DbSet<LoanDocuments> LoanDocuments => Set<LoanDocuments>();
        public DbSet<LoanDescriptions> LoanDescriptions => Set<LoanDescriptions>();
        public DbSet<LoanChangedCa> LoanChangedCa => Set<LoanChangedCa>();
        public DbSet<AppSettings> AppSettings => Set<AppSettings>();
        public DbSet<VUserLoanSizes> VUserLoanSizes => Set<VUserLoanSizes>();
        public DbSet<VUser> VUsers => Set<VUser>();
        public DbSet<VCurrenciesList> VCurrenciesList => Set<VCurrenciesList>();
        public DbSet<VCurrencyCounter> VCurrencyNotes => Set<VCurrencyCounter>();
        public DbSet<VBranch> VBranchs => Set<VBranch>();
        public DbSet<VDepartment> VDepartments => Set<VDepartment>();
        public DbSet<VSpotRate> VSpotRates => Set<VSpotRate>();
        public DbSet<VSchedule> VSchedules => Set<VSchedule>();
        public DbSet<VLoanRepaymentLine> VLoanRepaymentLines => Set<VLoanRepaymentLine>();
        public DbSet<VCollateral> VCollaterals => Set<VCollateral>();
        public DbSet<JournalEntryVm> VJournals => Set<JournalEntryVm>();
        public DbSet<VLedger> VLedgers => Set<VLedger>();
        public DbSet<VendorDto> VVendor => Set<VendorDto>();
        public DbSet<VGuarantor> VGuarantors => Set<VGuarantor>();
        public DbSet<VEmployeeCreditAgent> EmployeeCreditAgent => Set<VEmployeeCreditAgent>();
        public DbSet<VOtherNames> VOtherNames => Set<VOtherNames>();
        public DbSet<NameListVm> VNameLists => Set<NameListVm>();
        public DbSet<VEmployeeList> EmployeeLists => Set<VEmployeeList>();
        
        public DbSet<VCustomerDetails> VCustomerLists => Set<VCustomerDetails>();
        public DbSet<VCustomerNotes> VCustomerNotes => Set<VCustomerNotes>();
        public DbSet<VCustomerExpenses> VCustomerExpenses => Set<VCustomerExpenses>();
        public DbSet<VCustomerIncomes> VCustomerIncomes => Set<VCustomerIncomes>();
        public DbSet<VCustomerActivities> VCustomerActivities => Set<VCustomerActivities>();
        public DbSet<VCustomerCollateralLine> VCustomerCollateralLines => Set<VCustomerCollateralLine>();
        public DbSet<VCollateralLines> VCollateralLines => Set<VCollateralLines>(); 
        public DbSet<VLoanRequests> VLoanRequests => Set<VLoanRequests>();
        public DbSet<VLoanDisbursments> VLoanDisbursements => Set<VLoanDisbursments>();
        public DbSet<VCreditAgent> VCreditAgents => Set<VCreditAgent>();
        public DbSet<VDistricts> VDistricts => Set<VDistricts>();
        public DbSet<VCommunes> VCommunes => Set<VCommunes>();
        public DbSet<VVillages> VVillages => Set<VVillages>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<LtDocument> LtDocuments => Set<LtDocument>();
    }
}
