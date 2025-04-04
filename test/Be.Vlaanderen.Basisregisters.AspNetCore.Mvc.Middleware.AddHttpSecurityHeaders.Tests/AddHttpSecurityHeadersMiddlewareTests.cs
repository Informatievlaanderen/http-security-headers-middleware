namespace Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Middleware.AddHttpSecurityHeaders.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Xunit;

    public class AddHttpSecurityHeadersMiddlewareTests
    {
        [Fact]
        public async Task AddsExpectedHeadersToResponse()
        {
            var expectedServerName = "expectedName";
            var expectedPoweredBy = "expectedPowered";
            var middleware = new AddHttpSecurityHeadersMiddleware(innerContext => Task.CompletedTask, expectedServerName, expectedPoweredBy);

            var context = new DefaultHttpContext();
            context.Response.Headers.Append("X-Powered-By", "power!");

            await middleware.Invoke(context);

            context.Response.Headers.Count.Should().Be(5);

            context.Response.Headers.ContainsKey(AddHttpSecurityHeadersMiddleware.PoweredByHeaderName).Should().BeTrue();
            context.Response.Headers[AddHttpSecurityHeadersMiddleware.PoweredByHeaderName].Should().BeEquivalentTo(expectedPoweredBy);

            context.Response.Headers.ContainsKey(AddHttpSecurityHeadersMiddleware.ServerHeaderName).Should().BeTrue();
            context.Response.Headers[AddHttpSecurityHeadersMiddleware.ServerHeaderName].Should().BeEquivalentTo(expectedServerName);

            context.Response.Headers.ContainsKey(AddHttpSecurityHeadersMiddleware.ContentTypeOptionsHeaderName).Should().BeTrue();
            context.Response.Headers[AddHttpSecurityHeadersMiddleware.ContentTypeOptionsHeaderName].Should().BeEquivalentTo("nosniff");

            context.Response.Headers.ContainsKey(AddHttpSecurityHeadersMiddleware.FrameOptionsHeaderName).Should().BeTrue();
            context.Response.Headers[AddHttpSecurityHeadersMiddleware.FrameOptionsHeaderName].Should().BeEquivalentTo("DENY");

            context.Response.Headers.ContainsKey(AddHttpSecurityHeadersMiddleware.XssProtectionHeaderName).Should().BeTrue();
            context.Response.Headers[AddHttpSecurityHeadersMiddleware.XssProtectionHeaderName].Should().BeEquivalentTo("1; mode=block");
        }

        [Fact]
        public async Task AddFrameOptionsSameOrigin()
        {
            var middleware = new AddHttpSecurityHeadersMiddleware(innerContext => Task.CompletedTask, "server", "powered", FrameOptionsDirectives.SameOrigin);

            var context = new DefaultHttpContext();
            await middleware.Invoke(context);

            context.Response.Headers.ContainsKey(AddHttpSecurityHeadersMiddleware.FrameOptionsHeaderName).Should().BeTrue();
            context.Response.Headers[AddHttpSecurityHeadersMiddleware.FrameOptionsHeaderName].Should().BeEquivalentTo("SAMEORIGIN");
        }
    }
}
