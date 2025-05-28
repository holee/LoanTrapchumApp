using Applications.DependencyInjections;
using Applications.Extensions.Identity;
using Applications.Validations;
using Infrastructures.Authorizations;
using Infrastructures.DependencyInjects;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Infrastructure;
using Radzen;
using TrapchumApp;
using TrapchumApp.Components;
using TrapchumApp.Components.Account;
var builder = WebApplication.CreateBuilder(args);




QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddPolicyServices();
builder.Services.AddApplicationServices();
builder.Services.AddValidationServiceInjections();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddMapster();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<UserServiceImpl>();
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthorizationHandler, SameUserRequestAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SameUserApprovedAuthorizationHandler>();

var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{

//    app.UseExceptionHandler("/Error", createScopeForErrors: true);

//    app.UseHsts();
//}


await app.SeedSystemData();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapControllers();
await app.MapAdditionalIdentityEndpoints();

app.Run();
