namespace Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Add best practices security headers to every response.
    /// </summary>
    public class AddHttpSecurityHeadersMiddleware
    {
        public static string ServerHeaderName = "Server";
        public static string PoweredByHeaderName = "X-Powered-By";
        public static string ContentTypeOptionsHeaderName = "X-Content-Type-Options";
        public static string FrameOptionsHeaderName = "X-Frame-Options";
        public static string XssProtectionHeaderName = "X-XSS-Protection";

        private readonly RequestDelegate _next;
        private readonly string _serverName;
        private readonly string _poweredByName;
        private readonly FrameOptionsDirectives _frameOptionsDirectives;

        public AddHttpSecurityHeadersMiddleware(
            RequestDelegate next,
            string serverName = "Vlaamse overheid",
            string poweredByName = "Vlaamse overheid - Basisregisters Vlaanderen")
        : this(next, serverName, poweredByName, FrameOptionsDirectives.Deny)
        { }

        public AddHttpSecurityHeadersMiddleware(
            RequestDelegate next,
            string serverName = "Vlaamse overheid",
            string poweredByName = "Vlaamse overheid - Basisregisters Vlaanderen",
            FrameOptionsDirectives frameOptionsDirectives = FrameOptionsDirectives.Deny)
        {
            _next = next;
            _serverName = serverName;
            _poweredByName = poweredByName;
            _frameOptionsDirectives = frameOptionsDirectives;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Remove(ServerHeaderName);
            context.Response.Headers.Remove(PoweredByHeaderName);

            context.Response.Headers.Append(ServerHeaderName, _serverName);
            context.Response.Headers.Append(PoweredByHeaderName, _poweredByName);
            context.Response.Headers.Append(ContentTypeOptionsHeaderName, "nosniff");

            switch (_frameOptionsDirectives)
            {
                case FrameOptionsDirectives.Deny:
                    context.Response.Headers.Append(FrameOptionsHeaderName, "DENY");
                    break;
                case FrameOptionsDirectives.SameOrigin:
                    context.Response.Headers.Append(FrameOptionsHeaderName, "SAMEORIGIN");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("frameOptionsDirectives", _frameOptionsDirectives, "FrameOptionsDirective can only be Deny or SameOrigin.");
            }

            context.Response.Headers.Append(XssProtectionHeaderName, "1; mode=block");

            return _next(context);
        }
    }
}
