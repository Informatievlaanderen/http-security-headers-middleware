namespace Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Middleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Add best practices security headers to every response.
    /// </summary>
    public class AddHttpSecurityHeadersMiddleware
    {
        public static string ServerHeaderName = "Server";
        public static string PoweredByHeaderName = "x-powered-by";
        public static string ContentTypeOptionsHeaderName = "x-content-type-options";
        public static string FrameOptionsHeaderName = "x-frame-options";
        public static string XssProtectionHeaderName = "x-xss-protection";

        private readonly RequestDelegate _next;
        private readonly string _serverName;
        private readonly string _poweredByName;

        public AddHttpSecurityHeadersMiddleware(
            RequestDelegate next,
            string serverName = "Vlaamse overheid",
            string poweredByName = "Vlaamse overheid - Basisregisters Vlaanderen")
        {
            _next = next;
            _serverName = serverName;
            _poweredByName = poweredByName;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Remove(ServerHeaderName);
            context.Response.Headers.Remove(PoweredByHeaderName);

            context.Response.Headers.Add(ServerHeaderName, _serverName);
            context.Response.Headers.Add(PoweredByHeaderName, _poweredByName);
            context.Response.Headers.Add(ContentTypeOptionsHeaderName, "nosniff");
            context.Response.Headers.Add(FrameOptionsHeaderName, "DENY");
            context.Response.Headers.Add(XssProtectionHeaderName, "1; mode=block");

            return _next(context);
        }
    }
}
