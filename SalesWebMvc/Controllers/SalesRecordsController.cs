using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Services.ServiceModels;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly WebApiService _webApiService;

        public SalesRecordsController(WebApiService webApiService)
        {
            _webApiService = webApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            var includeList = new List<string> { "Seller", "Seller.Department" };

            var request = new SalesRecordByDateIncluding
            {
                MinDate = minDate,
                MaxDate = maxDate,
                IncludeList = includeList,
                GroupBySellerDepartment = false
            };

            string jsonValues = JsonConvert.SerializeObject(request);

            var result = await _webApiService.FindByDateAsync<SalesRecord>(jsonValues);

            ViewData["minDate"] = minDate?.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate?.Date.ToString("yyyy-MM-dd");

            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            var includeList = new List<string> { "Seller", "Seller.Department" };

            var request = new SalesRecordByDateIncluding
            {
                MinDate = minDate,
                MaxDate = maxDate,
                IncludeList = includeList,
                GroupBySellerDepartment = true
            };

            string jsonValues = JsonConvert.SerializeObject(request);

            var result = await _webApiService.FindByDateGroupingAsync<SalesRecord>(jsonValues);

            ViewData["minDate"] = minDate?.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate?.Date.ToString("yyyy-MM-dd");

            return View(result);
        }
    }
}