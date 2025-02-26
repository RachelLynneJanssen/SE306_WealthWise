using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
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
            if (model.CalculateInterest())
            {
                ViewData["InterestAmount"] = model.InterestAmount;
            }
            else
            {
                ViewData["InterestAmount"] = "Invalid input";
            }

            if (model.CalculateRetirement())
            {
                ViewData["RetirementAmount"] = model.RetirementAmount;
            }
            else
            {
                ViewData["RetirementAmount"] = "Invalid input";
            }

            return View(model);
        }
    }
}
