using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);

            ViewData["minDate"] = minDate?.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate?.Date.ToString("yyyy-MM-dd");

            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);

            ViewData["minDate"] = minDate?.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate?.Date.ToString("yyyy-MM-dd");

            return View(result);
        }
    }
}