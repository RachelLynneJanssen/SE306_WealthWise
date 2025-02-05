using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;

namespace WealthWise_RCD.Controllers
{
    public class FinancialCalcsController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Method == "POST")
            {
                string loanAmountStr = String.Format("{0}", Request.Form["loanAmount"]);
                string interestRateStr = String.Format("{0}", Request.Form["interestRate"]);
                string numberOfPaymentsStr = String.Format("{0}", Request.Form["numberOfPayments"]);
                string timeStr = String.Format("{0}", Request.Form["time"]);

                if (double.TryParse(loanAmountStr, out double loanAmount) &&
                    double.TryParse(interestRateStr, out double interestRate) &&
                    double.TryParse(numberOfPaymentsStr, out double numberOfPayments) &&
                    double.TryParse(timeStr, out double time))
                {
                    double amount = loanAmount * Math.Pow(1 + (interestRate/numberOfPayments), (time*numberOfPayments));
                    System.Diagnostics.Debug.WriteLine(amount);
                    ViewData["Amount"] = amount;
                }
                else
                {
                    ViewData["Amount"] = "Invalid input";
                    System.Diagnostics.Debug.WriteLine("Invalid input");
                }
            }
            

            return View();
        }
    }
}
