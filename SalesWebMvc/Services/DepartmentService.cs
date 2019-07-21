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
    public class DepartmentService
    {        
        public async Task<List<Department>> FindAllWebApiAsync()
        {
            var response = await new WebApiClient().GetAsync("api/departments");

            if (response.IsSuccessStatusCode)
            {
                return await response.To<List<Department>>();
            }

            return new List<Department>();
        }

        public async Task<Department> FindByIdAsync(int id)
        {
            var response = await new WebApiClient().GetAsync($"api/departments/{id}");

            if (response.IsSuccessStatusCode)
                return await response.To<Department>();

            return null;
        }

        public async Task UpdateAsync(int id, string jsonValues)
        {
            var response = await new WebApiClient().PutAsync($"api/departments/{id}", jsonValues);
        }

        public async Task InsertAsync(string jsonValues)
        {
            var response = await new WebApiClient().PostAsync($"api/departments", jsonValues);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await new WebApiClient().DeleteAsync($"api/departments/{id}");
        }        
    }
}
