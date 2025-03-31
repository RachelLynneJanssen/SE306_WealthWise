using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]
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
                ViewData["ActiveTab"] = "investment";
                ViewData["InterestAmount"] = model.InterestAmount;
            }
            else
            {
                ViewData["InterestAmount"] = "Invalid input";
            }

            if (model.CalculateRetirement())
            {
                ViewData["ActiveTab"] = "retirement";
                ViewData["RetirementAmount"] = model.RetirementAmount;
                ViewData["YearlySavings"] = model.YearlySavings;
            }
            else
            {
                ViewData["RetirementAmount"] = "Invalid input";
            }

            return View(model);
        }

        public IActionResult MonthlyBudgetCreator()
        {
            return View();
        }
    }
}
