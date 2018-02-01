using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models.ReportingViewModels;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    [Route("api/reporting")]
    [Produces("application/json")]
    public class ReportingAPIController : Controller
    {
        private readonly IReportingRepository _reportingRepo;

        public ReportingAPIController(IReportingRepository reportingRepo)
        {
            _reportingRepo = reportingRepo;
        }

        //GET --api/reporting
        [HttpGet]
        public SalesViewModel Index()
        {
            var vm = new SalesViewModel
            {
                SalesByProduct = _reportingRepo.TopSellingProducts(3),
                SalesByCategory = _reportingRepo.SalesByCategory(3),
                YearSalesByMonth = _reportingRepo.LastYearSalesByMonth(),
                YearSales = _reportingRepo.LastYearSales(),
                From = DateTime.Now.AddMonths(-11),
                To = DateTime.Now
            };

            return vm;
        }
    }
}