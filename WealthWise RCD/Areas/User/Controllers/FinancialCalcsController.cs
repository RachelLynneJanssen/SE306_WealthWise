using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using WealthWise_RCD.Services;
using WealthWise_RCD.Models.DatabaseModels;
using Microsoft.AspNetCore.Identity;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]
    public class FinancialCalcsController : Controller
    {
        private readonly MonthlyBudgetService _monthlyBudgetService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FinancialCalcsController(MonthlyBudgetService monthlyBudgetService,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
            var getUser = _userManager.GetUserAsync(User);
            getUser.Wait();
            ApplicationUser applicationUser = getUser.Result;

            ViewData["MonthlyBudgets"] = _monthlyBudgetService.GetAllMonthlyBudgetsAsync(applicationUser.Id).Result; // Get all monthly budgets to display in the view

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyBudgetUpdater(MonthlyBudgetCalculator model)
        {
            if (!ModelState.IsValid || !model.checkInput())
            {
                // Return the view with the model to show validation errors
                ViewData["ErrorMessage"] = "Please check your inputs. Income, Expense and Savings must be greater than 0.";
                return View(model);
            }

            MonthlyBudget monthlyBudget = _monthlyBudgetService.GetLatestMonthlyBudgetAsync().Result;

            var getUser = _userManager.GetUserAsync(User);
            getUser.Wait();
            ApplicationUser applicationUser = getUser.Result;

            if (monthlyBudget == null)
            {
                // If no monthly budget exists, create a new one
                monthlyBudget = new MonthlyBudget
                {
                    Income = model.Income,
                    Expense = model.Expense,
                    Savings = model.Savings,
                    UserID = applicationUser.Id,
                    Total = model.Income - model.Expense - model.Savings,
                    CreatedDate = DateTime.Now
                };
            }
            else
            {
                monthlyBudget.Income += model.Income;
                monthlyBudget.Expense += model.Expense;
                monthlyBudget.Savings += model.Savings;
                monthlyBudget.Total = monthlyBudget.Income - monthlyBudget.Expense - monthlyBudget.Savings;
            }



            await _monthlyBudgetService.UpsertMonthlyBudgetPostAsync(monthlyBudget);

            ViewData["CurrentBudget"] = "Monthly Budget Updated Successfully! " +
                $"Current Income: {monthlyBudget.Income}, Current Expense: {monthlyBudget.Expense}, Current Savings: {monthlyBudget.Savings}";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBudget()
        {
            var getUser = _userManager.GetUserAsync(User);
            getUser.Wait();

            ApplicationUser applicationUser = getUser.Result;
            // If no monthly budget exists, create a new one
            MonthlyBudget monthlyBudget = new MonthlyBudget
            {
                Income = 0,
                Expense = 0,
                Savings = 0,
                UserID = applicationUser.Id,
                Total = 0,
                CreatedDate = DateTime.Now
            };

            await _monthlyBudgetService.UpsertMonthlyBudgetPostAsync(monthlyBudget);

            ViewData["CurrentBudget"] = "New Monthly Budget Created Successfully! " +
                $"at {DateTime.Now}";

            return RedirectToAction("MonthlyBudgetUpdater");
        }
    }
}
