using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Revlucio.NtlmAuthentication
{
    public class NtlmAuthenticationMiddleware
    {
        private RequestDelegate _next;

        public NtlmAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = Convert.FromBase64String(context.Request.Headers["Authorization"]);
                if (token.Length > 3 && token.Length < 91)
                {
                    // handshake state 2 - CHALLENGE
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.Headers.Add("WWW-Authenticate", "NTLM TlRMTVNTUAACAAAAHgAeADgAAAAFwoqi4OsyKS+KwHxArb4Q4wAAAJgAmABWAAAACgAAKAAAAA9EAEUAUwBLAFQATwBQAC0ANwBRAEsANABLAEMASgACAB4ARABFAFMASwBUAE8AUAAtADcAUQBLADQASwBDAEoAAQAeAEQARQBTAEsAVABPAFAALQA3AFEASwA0AEsAQwBKAAQAHgBEAEUAUwBLAFQATwBQAC0ANwBRAEsANABLAEMASgADAB4ARABFAFMASwBUAE8AUAAtADcAUQBLADQASwBDAEoABwAIAAWsUmSNW9EBAAAAAA==");
                    return;
                } 
                else if (token.Length > 90)
                {
                    // handshake state 3 - AUTHENTICATED
                }
                else 
                {
                    // handshake state 1 - NEGOTIATE
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.Headers.Add("WWW-Authenticate", "NTLM");
                    return;
                }
            }
            else
            {
                // handshake state 1 - NEGOTIATE
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.Add("WWW-Authenticate", "NTLM");
                return;    
            }
            
            await _next(context);
        }
    }
}