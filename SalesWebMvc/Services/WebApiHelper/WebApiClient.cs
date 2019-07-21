using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesWebMvc.Services.WebApiHelper
{
    public class WebApiClient
    {
        HttpClient webApi;

        public WebApiClient()
        {
            webApi = new HttpClient();
            webApi.BaseAddress = new Uri("https://localhost:44395");
            webApi.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> GetAsync(string webApiRoute)
        {
            return await webApi.GetAsync(webApiRoute);
        }

        public async Task<HttpResponseMessage> PutAsync(string webApiRoute, string jsonValues)
        {
            return await webApi.PutAsync(webApiRoute, jsonValues, new JsonMediaTypeFormatter());
        }

        public async Task<HttpResponseMessage> PostAsync(string webApiRoute, string jsonValues)
        {
            return await webApi.PostAsync(webApiRoute, jsonValues, new JsonMediaTypeFormatter());
        }
        
        public async Task<HttpResponseMessage> DeleteAsync(string webApiRoute)
        {
            return await webApi.DeleteAsync(webApiRoute);
        }
    }
}
