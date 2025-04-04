using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using WealthWise_RCD.Services;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]
    public class FinancialCalcsController : Controller
    {
        private readonly MonthlyBudgetService _monthlyBudgetService;

        public FinancialCalcsController(MonthlyBudgetService monthlyBudgetService)
        {
            _monthlyBudgetService = monthlyBudgetService;
        }

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
        public IActionResult MonthlyBudgetUpdater(MonthlyBudgetCalculator model)
        {
            if (!ModelState.IsValid || !model.checkInput())
            {
                // Return the view with the model to show validation errors
                ViewData["ErrorMessage"] = "Please check your inputs. Income, Expense and Savings must be greater than 0.";
                return View(model);
            }

            MonthlyBudget monthlyBudget = _monthlyBudgetService.GetLatestMonthlyBudgetAsync().Result;

            monthlyBudget.Income += model.Income;
            monthlyBudget.Expense += model.Expense;
            monthlyBudget.Savings += model.Savings;

            _monthlyBudgetService.UpsertMonthlyBudgetPostAsync(monthlyBudget);

            ViewData["CurrentBudget"] = "Monthly Budget Updated Successfully! <br/> " +
                $"Current Income: {monthlyBudget.Income}, Current Expense: {monthlyBudget.Expense}, Current Savings: {monthlyBudget.Savings}";

            return View(model); 
        }
    }
}
