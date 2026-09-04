using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

namespace Web.Api.Authentication;

public static class OpenApiJwtExtensions
{
    /// <summary>
    /// Publishes a <c>bearer</c> security scheme in the generated OpenAPI document and
    /// attaches it to every operation that is protected by authorization metadata.
    /// </summary>
    public static IServiceCollection AddOpenApiWithJwtBearer(this IServiceCollection services)
    {
        return services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes[JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Paste the access token returned by POST /api/auth/login."
                };

                return Task.CompletedTask;
            });

            options.AddOperationTransformer((operation, context, _) =>
            {
                var metadata = context.Description.ActionDescriptor.EndpointMetadata;

                if (metadata.OfType<IAllowAnonymous>().Any() || !metadata.OfType<IAuthorizeData>().Any())
                {
                    return Task.CompletedTask;
                }

                operation.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, context.Document)] = []
                    }
                ];

                return Task.CompletedTask;
            });
        });
    }
}
