using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Services
{
    public class MonthlyBudgetService
    {
        private readonly ApplicationDbContext _context;

        public MonthlyBudgetService( ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyBudget>> GetAllMonthlyBudgetsAsync()
        {
            return await _context.MonthlyBudgets.ToListAsync();
        }

        public async Task UpsertMonthlyBudgetPostAsync(MonthlyBudget monthlyBudget)
        {
            if (monthlyBudget.Id == 0)
            {
                _context.MonthlyBudgets.Add(monthlyBudget);
            }
            else
            {
                var postEntry = await _context.MonthlyBudgets.FindAsync(monthlyBudget.Id);
                if (postEntry != null)
                {
                    postEntry.Income = monthlyBudget.Income;
                    postEntry.Savings = monthlyBudget.Savings;
                    postEntry.Expense = monthlyBudget.Expense;
                    postEntry.Total = monthlyBudget.Total;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<MonthlyBudget> GetLatestMonthlyBudgetAsync()
        {
            // Get the latest MonthlyBudget entry based on CreatedDate
            return await _context.MonthlyBudgets
                .OrderByDescending(m => m.CreatedDate)
                .FirstOrDefaultAsync();
        }
    }
}
