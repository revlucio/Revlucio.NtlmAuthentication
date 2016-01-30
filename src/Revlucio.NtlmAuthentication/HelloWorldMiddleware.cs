using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Revlucio.NtlmAuthentication
{
    public class HelloWorldMiddleware
    {
        private RequestDelegate _next;
        
        public HelloWorldMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context) 
        {
            await context.Response.WriteAsync("Hello world!");    
            await _next(context);
        }
    }
}