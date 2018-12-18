# Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Middleware.AddHttpSecurityHeaders

Middleware component which replaces and adds common security related http headers.

| Header                 | Value                                        |
| ---------------------- | -------------------------------------------- |
| Server                 | Vlaamse overheid                             |
| x-powered-by           | Vlaamse overheid - Basisregisters Vlaanderen |
| x-content-type-options | nosniff                                      |
| x-frame-options        | DENY                                         |
| x-xss-protection       | 1; mode=block                                |

## Usage

```csharp
public void Configure(IApplicationBuilder app, ...)
{
    app
        ...
        .UseMiddleware<AddHttpSecurityHeadersMiddleware>()
        ...
}
```
