using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SalesWebMvc.Models;
using System.Linq;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordsService _salesRecordsService;

        public SalesRecordsController(SalesRecordsService salesRecordsService)
        {
            _salesRecordsService = salesRecordsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if(!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Today.Year, 1, 1);
            }

            if(!maxDate.HasValue)
            {
                maxDate = DateTime.Today;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); // html5 date input format = yyyy-MM-dd
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            List<SalesRecord> sales = await _salesRecordsService.FindByDateAsync(minDate, maxDate);
            return View(sales);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Today.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Today;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); // html5 date input format = yyyy-MM-dd
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            List<IGrouping<Department, SalesRecord>> salesGroupedByDepartment = await _salesRecordsService.FindByDateGroupingAsync(minDate, maxDate);
            return View(salesGroupedByDepartment);
        }
    }
}
