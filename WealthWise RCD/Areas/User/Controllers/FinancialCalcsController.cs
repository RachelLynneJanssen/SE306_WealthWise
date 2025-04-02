using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

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

        public IActionResult MonthlyBudgetUpdater()
        {
            return View();
        }

        public IActionResult MonthlyBudgetViewer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MonthlyBudgetUpdater(MothlyBudgetCalculator model)
        {
            if(!ModelState.IsValid || !model.checkInput())
            {
                // Return the view with the model to show validation errors
                ViewData["ErrorMessage"] = "Please check your inputs. Income, Expense and Savings must be greater than 0.";
                return View(model);
            }
            return View(model); 
        }
    }
}
