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
            ViewData["ActiveTab"] = model.activeTab;

            if (model.activeTab == "interest")
            {
                if (model.CalculateInterest())
                {
                    ViewData["InterestAmount"] = model.InterestAmount;
                }
                else
                {
                    ViewData["InterestAmount"] = "Invalid input";
                }
            }

            if (model.activeTab == "retirement")
            {
                if (model.CalculateRetirement())
                {
                    ViewData["RetirementAmount"] = model.RetirementAmount;
                    ViewData["YearlySavings"] = model.YearlySavings;
                }
                else
                {
                    ViewData["RetirementAmount"] = "Invalid input";
                }
            }

            return View(model);
        }
    }
}