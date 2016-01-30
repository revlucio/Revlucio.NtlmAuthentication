using Microsoft.AspNet.Builder;

namespace Revlucio.NtlmAuthentication.Tests
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<HelloWorldMiddleware>();
        }
    }
}