using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.TestHost;
using Xunit;

namespace Revlucio.NtlmAuthentication.Tests
{
    public class NtmlAuthenicationMiddlewareShould
    {
        public TestServer CreateTestServer()
        {
            var server = TestServer.Create((app) =>
            {
                app.UseMiddleware<NtlmAuthenticationMiddleware>();
                app.UseMiddleware<HelloWorldMiddleware>();
            });
            
            return server;
        }
        
        public byte[] GetMyToken()
        {
            // this comes from my machine
            var tokenString = "TlRMTVNTUAABAAAAB7IIogkACQA3AAAADwAPACgAAAAKAAAoAAAAD0RFU0tUT1AtN1FLNEtDSldPUktHUk9VUA==";
            return Convert.FromBase64String(tokenString);
        }
        
        private byte[] GetAuthToken()
        {
            var authString = "TlRMTVNTUAADAAAAAAAAAFgAAAAAAAAAWAAAAAAAAABYAAAAAAAAAFgAAAAAAAAAWAAAAAAAAABYAAAABcKIogoAACgAAAAPnVp5ER7sL0ir52+YqHNLUA==";
            return Convert.FromBase64String(authString);
        }
        
        [Fact]
        public async void RespondWithNegotiateIfNoAuthenticationHeaderPresent()
        {
            using (var server = CreateTestServer())
            {
                var response = await server.CreateClient().GetAsync("/");
                
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                Assert.Equal("NTLM", response.Headers.WwwAuthenticate.Single().Scheme.ToString());
                Assert.Null(response.Headers.WwwAuthenticate.Single().Parameter);
            }
        }
        
        [Fact]
        public async void NotRespondWithChallengeIfNoNamePresent()
        {
            using (var server = CreateTestServer())
            {
                var emptyToken = String.Empty;
                
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri("http://localhost/");
                request.Headers.Authorization = new AuthenticationHeaderValue("NTLM", emptyToken);
                
                var response = await server.CreateClient().SendAsync(request);
                
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                Assert.Equal("NTLM", response.Headers.WwwAuthenticate.Single().Scheme.ToString());
                Assert.Null(response.Headers.WwwAuthenticate.Single().Parameter);
            }
        }
        
        [Fact]
        public async void ResponseWithChallengeIfNamePresent()
        {
            using (var server = CreateTestServer())
            {
                var token = Convert.ToBase64String(GetMyToken());
                
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri("http://localhost/");
                request.Headers.Authorization = new AuthenticationHeaderValue("NTLM", token);
                
                var expectedChallenge = "TlRMTVNTUAACAAAAHgAeADgAAAAFwoqi4OsyKS+KwHxArb4Q4wAAAJgAmABWAAAACgAAKAAAAA9EAEUAUwBLAFQATwBQAC0ANwBRAEsANABLAEMASgACAB4ARABFAFMASwBUAE8AUAAtADcAUQBLADQASwBDAEoAAQAeAEQARQBTAEsAVABPAFAALQA3AFEASwA0AEsAQwBKAAQAHgBEAEUAUwBLAFQATwBQAC0ANwBRAEsANABLAEMASgADAB4ARABFAFMASwBUAE8AUAAtADcAUQBLADQASwBDAEoABwAIAAWsUmSNW9EBAAAAAA==";
                
                var response = await server.CreateClient().SendAsync(request);
                
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                Assert.Equal("NTLM", response.Headers.WwwAuthenticate.Single().Scheme.ToString());
                Assert.Equal(expectedChallenge, response.Headers.WwwAuthenticate.Single().Parameter.ToString());
            }
        }
        
        [Fact]
        public async void RespondWithOkIfAuthHeaderPresent()
        {
            using (var server = CreateTestServer())
            {
                var token = Convert.ToBase64String(GetAuthToken());
                
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri("http://localhost/");
                request.Headers.Authorization = new AuthenticationHeaderValue("NTLM", token);
                
                var response = await server.CreateClient().SendAsync(request);
                
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}