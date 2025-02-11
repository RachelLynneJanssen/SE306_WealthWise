using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
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
            if (model.Calculate())
            {
                ViewData["Amount"] = model.Amount;
            }
            else
            {
                ViewData["Amount"] = "Invalid input";
            }

            return View(model);
        }
    }
}
