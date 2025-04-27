using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;
using WealthWise_RCD.Services;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    [Authorize(Roles = "Advisor,Admin")]
    public class FinancialCalcsController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View(new FinancialCalculator());
        }

        [HttpPost]
        public IActionResult Index(FinancialCalculator model)
        {

            return View(model);
        }
    }
}