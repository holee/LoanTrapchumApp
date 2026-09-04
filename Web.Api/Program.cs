using Applications;
using Infrastructures;
using Mapster;
using Web.Api.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddValidationServices();
builder.Services.AddMapster();
builder.Services.AddHttpContextAccessor();

// Must run after AddInfrastructureServices, which registers the Identity cookie schemes:
// this re-points the default authenticate/challenge schemes to JWT bearer.
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiWithJwtBearer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers().WithStaticAssets();

app.Run();
