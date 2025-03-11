using System.Data;
using System.Reflection;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Currencies;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Departments;
using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Responses.ViewModels.Loans;
using Domains.Models;
using Domains.Models.Addresses;
using Domains.Models.Auth;
using Domains.Models.Collaterals;
using Domains.Models.Currencies;
using Domains.Models.Journals;
using Domains.Models.Lists;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Domains.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructures.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
{ }

public IQueryable<VCurrencyNotes> GetCurrencyNotes(int branchId) => FromExpression(() => GetCurrencyNotes(branchId));

protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    foreach (var property in builder.Model.GetEntityTypes()
           .SelectMany(t => t.GetProperties())
           .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
    {
        property.SetColumnType("decimal(18,2)");
    }

    builder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(GetCurrencyNotes), new[] { typeof(int) })!);

    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

}

private void ProcessSaveChangedAsync()
{
    foreach (var item in ChangeTracker.Entries()
                        .Where(e => e.Entity is BaseAddress && e.State == EntityState.Added))
    {
        BaseAddress baseEntry = (BaseAddress)item.Entity;
        baseEntry.CreatedDt = DateTime.Now;
        baseEntry.ModifiedDt = DateTime.Now;
    }

    foreach (var item in ChangeTracker.Entries()
                       .Where(e => e.Entity is BaseAddress && e.State == EntityState.Modified))
    {
        var baseEntries = (BaseAddress)item.Entity;
        baseEntries.ModifiedDt = DateTime.Now;
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
        baseEntries.ModifiedDt = DateTime.Now;
        baseEntries.EditSeq += 1;
        item.Property(nameof(BaseEntity.CreatedDt)).IsModified = false;

    }
}
private void ProcessSaveChanged(bool acceptAllChangesOnSuccess)
{
    foreach (var item in ChangeTracker.Entries()
                        .Where(e => e.Entity is BaseAddress && e.State == EntityState.Added))
    {
        BaseAddress baseEntry = (BaseAddress)item.Entity;
        baseEntry.CreatedDt = DateTime.Now;
        baseEntry.ModifiedDt = DateTime.Now;
    }
    foreach (var item in ChangeTracker.Entries()
                       .Where(e => e.Entity is BaseAddress && e.State == EntityState.Modified))
    {
        var baseEntries = (BaseAddress)item.Entity;
        baseEntries.ModifiedDt = DateTime.Now;
        baseEntries.EditSeq += 1;
        item.Property(nameof(BaseAddress.CreatedDt)).IsModified = false;

    }
    ////for Entity inherit from BaseEntity
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
        baseEntries.ModifiedDt = DateTime.Now;
        baseEntries.EditSeq += 1;
        item.Property(nameof(BaseEntity.CreatedDt)).IsModified = false;

    }
}

public IDbTransaction StartTransaction()
{
    var transaction = Database.BeginTransaction();
    return transaction.GetDbTransaction();
}

public DbSet<Bank> Banks { get; set; }
public DbSet<Currency> Currencies { get; set; }
public DbSet<CurrencyCounter> CurrencyCounters { get; set; }
public DbSet<Branch> Branches { get; set; }
//public DbSet<RefreshToken> RefreshTokens { get; set; }
public DbSet<Collateral> Collaterals { get; set; }
public DbSet<CollateralLink> CollateralsLinks { get; set; }
public DbSet<CollateralLine> CollateralLines { get; set; }
public DbSet<CollateralEvaluations> CollateralEvaluations { get; set; }
public DbSet<CollateralType> CollateralTypes { get; set; }
public DbSet<CollateralSubType> CollateralSubTypes { get; set; }
public DbSet<LoanPropertyType> LoanPropertyType { get; set; }
public DbSet<Department> Departments { get; set; }

public DbSet<UserLoanSize> UserLoanSizes { get; set; }
public DbSet<Salutation> Salutations { get; set; }
public DbSet<Employee> Employees { get; set; }
public DbSet<LoanCreditAgent> LoanCreditAgents { get; set; }
public DbSet<Customer> Customers { get; set; }
public DbSet<CustomerExpense> CustomerExpenses { get; set; }
public DbSet<CustomerIncome> CustomerIncomes { get; set; }
public DbSet<EntityName> Vendors { get; set; }
public DbSet<EntityName> OtherNames { get; set; }
public DbSet<Occupation> Occupations { get; set; }
public DbSet<Address> Addresses { get; set; }
public DbSet<City> Cities { get; set; }
public DbSet<Country> Countries { get; set; }
public DbSet<Province> Provinces { get; set; }
public DbSet<District> Districts { get; set; }
public DbSet<Commune> Communes { get; set; }
public DbSet<CustomerNote> CustomerNote { get; set; }
public DbSet<Village> Villages { get; set; }
public DbSet<Nationality> Nationalities { get; set; }
public DbSet<Ledger> Ledgers { get; set; }
public DbSet<LedgerType> LedgerTypes { get; set; }
public DbSet<JournalEntry> JournalEntries { get; set; }
public DbSet<JournalEntryLine> JournalEntryLines { get; set; }
public DbSet<JournalType> JournalTypes { get; set; }
public DbSet<Loan> LoanDisbursments { get; set; }
public DbSet<LoanRepayment> LoanRepayments { get; set; }
public DbSet<LoanRepaymentLine> LoanRepaymentLine { get; set; }
public DbSet<LoanSchedule> LoanSchedules { get; set; }
public DbSet<LoanStatus> LoanStatuses { get; set; }
public DbSet<LoanCategory> LoanCategories { get; set; }
public DbSet<LoanProduct> LoanProducts { get; set; }
public DbSet<LoanClarifyOfStaff> LoanClarifyOfStaff { get; set; }
public DbSet<LoanRepaymentMode> LoanRepaymentModes { get; set; }
public DbSet<LoanInstallmentType> LoanInstallmentTypes { get; set; }
public DbSet<LoanTransaction> LoanTransaction { get; set; }
public DbSet<LoanTransactionType> TransactionTypes { get; set; }
public DbSet<LoanScheduleStatus> LoanScheduleStatuses { get; set; }
public DbSet<LoanPaymentLocation> LoanPaymentLocations { get; set; }
public DbSet<OpenBranch> OpenBranches { get; set; }
public DbSet<CashVault> CashVaults { get; set; }
public DbSet<CashManagement> CashManagements { get; set; }
public DbSet<CashManagementDetail> CashManagementDetails { get; set; }
public DbSet<CashStatus> CashStatuses { get; set; }
public DbSet<ForeignExchange> ForeignExchanges { get; set; }
public DbSet<LoanTermType> LoanTerms { get; set; }
public DbSet<LoanTypeStatus> LoanSubTypeStatuses { get; set; }
public DbSet<LoanSubTypeStatus> LoanSubTypeNoAcc { get; set; }
public DbSet<GuarantorLink> Guarantors { get; set; }
public DbSet<CoPawnCustomer> CoPawnCustomers { get; set; }
public DbSet<AppStatus> AppStatuses { get; set; }
public DbSet<CurrencySpotRate> SpotRates { get; set; }
public DbSet<ActivitiesStatus> ActivitiesStatuses { get; set; }
public DbSet<BankOther> BankOthers { get; set; }
public DbSet<DayName> DayNames { get; set; }
public DbSet<Holiday> Holidays { get; set; }
public DbSet<MonthName> MonthNames { get; set; }
public DbSet<OtherType> OtherTypes { get; set; }
public DbSet<MarritalStatus> MarritalStatuses { get; set; }
public DbSet<Loan> LoanDisbursementRequests { get; set; }
public DbSet<LoanRequest> LoanRequests { get; set; }
public DbSet<LoanDocuments> LoanDocuments { get; set; }
public DbSet<LoanChangedCa> LoanChangedCa { get; set; }
public DbSet<SysSettings> AppSettings { get; set; }

/// <summary>
/// Link to Views
/// </summary>
public DbSet<VCurrenciesList> VCurrenciesList { get; set; }
public DbSet<VCurrencyNotes> VCurrencyNotes { get; set; }
public DbSet<VBranch> VBranchs { get; set; }
public DbSet<VDepartment> VDepartments { get; set; }
public DbSet<VSpotRate> VSpotRates { get; set; }
public DbSet<VLoanDisbursments> VLoanDisbursements { get; set; }
public DbSet<VLoanRepaymentLine> VLoanRepaymentLines { get; set; }
public DbSet<VCollateral> VCollaterals { get; set; }
public DbSet<VJournal> VJournals { get; set; }
public DbSet<VLedger> VLedgers { get; set; }
public DbSet<VNameList> NameLists { get; set; }
public DbSet<VEmployees> EmployeeLists { get; set; }
public DbSet<VCustomerList> CustomerLists { get; set; }
public DbSet<VCreditAgent> VCreditAgents { get; set; }
    }
}
