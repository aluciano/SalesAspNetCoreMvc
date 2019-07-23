using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesWebMvc.Models.Extensions
{
    static class HttpResponseMessageExtension
    {
        public static async Task<T> To<T>(this HttpResponseMessage response)
        {
            try
            {
                var webApiEntity = await response.Content.ReadAsStringAsync();

                var entity = JsonConvert.DeserializeObject<T>(webApiEntity);

                return entity;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
