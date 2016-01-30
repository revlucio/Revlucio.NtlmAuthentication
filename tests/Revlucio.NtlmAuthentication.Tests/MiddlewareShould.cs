using System.Net;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.TestHost;
using Xunit;

namespace Revlucio.NtlmAuthentication.Tests
{
    public class MiddlewareShould
    {
        [Fact]
        public async void PrintHelloWorld()
        {
            var server = TestServer.Create((app) =>
            {
                app.UseMiddleware<HelloWorldMiddleware>();
            });

            using (server)
            {
                var response = await server.CreateClient().GetAsync("/");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}