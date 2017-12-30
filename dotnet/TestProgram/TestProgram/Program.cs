
namespace TestProgram
{
    using System;
    using System.Net;
    using System.Net.Http;

    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            var httpClient = new HttpClient();

            // for prod env
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://falconhypernet.glbdns2.microsoft.com:443/keepalive");
            /*
            httpRequestMessage.Headers.Host = "serviceprober.asgfalcon.io";
            */

            /*
            // for prod westus2 env
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://falcon-prod-wus2.binginternal.com:443/keepalive");
            httpRequestMessage.Headers.Host = "serviceprober.asgfalcon.io";
            */

            /*
            // for testing env
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://fabricrouter.asgfalcon-test.io:443/keepalive");
            httpRequestMessage.Headers.Host = "serviceprober.asgfalcon-test.io";
            */

            /*
            // for staging env
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://falcon-staging-eus2.binginternal.com:443/keepalive");
            httpRequestMessage.Headers.Host = "helloworld.asgfalcon-staging.io";
            */

            var httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;
            var statusCode = httpResponseMessage.StatusCode;
            var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Status: {0}, Content: {1}", statusCode, content);

            Console.ReadKey();
        }
    }
}
