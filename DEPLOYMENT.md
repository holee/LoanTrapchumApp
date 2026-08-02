# Production deployment

The application intentionally does not store production credentials in source control. Before starting a production instance, set these environment variables through the deployment platform's secret store:

```text
ConnectionStrings__MyConnection=<SQL Server connection string>
AllowedHosts=<semicolon-separated public host names>
```

Use HTTPS at the application or a trusted reverse proxy. The app now has an authenticated, antiforgery-protected upload API, so unauthenticated clients must not call `/Upload/*`.

Rotate the database password that was previously committed to `TrapchumApp/appsettings.json`; removing it from the current file does not remove it from Git history.
