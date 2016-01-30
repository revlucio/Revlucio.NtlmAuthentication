using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.Net.Http.Server;

namespace Revlucio.NtlmAuthentication.Tests
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var listener = app.ServerFeatures.Get<WebListener>();
            if (listener != null)
            {
                listener.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.NTLM;
            } 
            else 
            {
                app.UseMiddleware<NtlmAuthenticationMiddleware>();
                app.UseMiddleware<HelloWorldMiddleware>();    
            }
            
            app.Run(context => context.Response.WriteAsync("test"));
        }
    }
}