using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Extensions;
using SalesWebMvc.Services.WebApiHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class WebApiService
    {
        private readonly WebApiClient WebApi;

        public WebApiService(WebApiClient webApi)
        {
            WebApi = webApi;
        }

        private string ControllerName<T>()
        {
            return $"{typeof(T).Name}s".ToLower();
        }

        public async Task<List<T>> FindAllAsync<T>()
        {
            var response = await WebApi.GetAsync($"api/{ControllerName<T>()}");

            if (response.IsSuccessStatusCode)
            {
                return await response.To<List<T>>();
            }

            return new List<T>();
        }

        public async Task<T> FindByIdAsync<T>(int id)
        {
            var response = await WebApi.GetAsync($"api/{ControllerName<T>()}/{id}");

            if (response.IsSuccessStatusCode)
                return await response.To<T>();

            return default;
        }

        public async Task<T> FindByIdIncludingAsync<T>(int id, string classToInclude)
        {
            try
            {
                var response = await WebApi.GetAsync($"api/{ControllerName<T>()}/{id}/{classToInclude}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.To<T>();
                }

                return default;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task UpdateAsync<T>(int id, string jsonValues)
        {
            var response = await WebApi.PutAsync($"api/{ControllerName<T>()}/{id}", jsonValues);
        }

        public async Task InsertAsync<T>(string jsonValues)
        {
            var response = await WebApi.PostAsync($"api/{ControllerName<T>()}", jsonValues);
        }

        public async Task DeleteAsync<T>(int id)
        {
            var response = await WebApi.DeleteAsync($"api/{ControllerName<T>()}/{id}");
        }
    }
}
