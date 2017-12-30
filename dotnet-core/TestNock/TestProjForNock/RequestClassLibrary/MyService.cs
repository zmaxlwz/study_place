// copy right info

namespace RequestClassLibrary
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class MyService
    {
        public async Task<HttpResponseMessage> GetResponse(string url)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            return response;
        }

    }
}
