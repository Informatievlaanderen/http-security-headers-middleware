namespace Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Middleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Add best practices security headers to every response.
    /// </summary>
    public class AddHttpSecurityHeadersMiddleware
    {
        public static string PoweredByHeaderName = "x-powered-by";
        public static string ContentTypeOptionsHeaderName = "x-content-type-options";
        public static string FrameOptionsHeaderName = "x-frame-options";
        public static string XssProtectionHeaderName = "x-xss-protection";

        private readonly RequestDelegate _next;

        public AddHttpSecurityHeadersMiddleware(RequestDelegate next) => _next = next;

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");

            context.Response.Headers.Add("Server", "Vlaamse overheid");

            context.Response.Headers.Add(PoweredByHeaderName, "Vlaamse overheid - Basisregisters Vlaanderen");
            context.Response.Headers.Add(ContentTypeOptionsHeaderName, "nosniff");
            context.Response.Headers.Add(FrameOptionsHeaderName, "DENY");
            context.Response.Headers.Add(XssProtectionHeaderName, "1; mode=block");

            return _next(context);
        }
    }
}
