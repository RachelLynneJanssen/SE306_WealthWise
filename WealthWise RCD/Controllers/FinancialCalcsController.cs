using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;

namespace WealthWise_RCD.Controllers
{
    public class FinancialCalcsController : Controller
    {
        public IActionResult Index()
        {
            // Check if the http request is a POST request
            if (Request.Method == "POST")
            {
                string initialAmountStr = String.Format("{0}", Request.Form["initialAmount"]);
                string interestRateStr = String.Format("{0}", Request.Form["interestRate"]);
                string numberOfPaymentsStr = String.Format("{0}", Request.Form["numberOfPayments"]);
                string timeStr = String.Format("{0}", Request.Form["time"]);

                //check vaild format and convert to double
                if (double.TryParse(initialAmountStr, out double loanAmount) &&
                    double.TryParse(interestRateStr, out double interestRate) &&
                    double.TryParse(numberOfPaymentsStr, out double numberOfPayments) &&
                    double.TryParse(timeStr, out double time))
                {
                    // calculate the amount
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
