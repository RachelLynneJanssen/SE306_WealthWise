using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]
    public class SchedulerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            var testAppointments = new List<Appointment>
            {
                new Appointment { Id = 1, UserId = currentUser.Id, ScheduledTime = DateTime.Now.AddDays(1), AdvisorId = "10000"},
                new Appointment { Id = 2, UserId = currentUser.Id, ScheduledTime = DateTime.Now.AddDays(2), AdvisorId = "10001"}
            };

            var appointments = await _context.Appointments
                .Where(a => a.UserId == currentUser.Id)
                .OrderBy(a => a.ScheduledTime)
                .ToListAsync();

            ViewBag.UserName = currentUser.UserName;
            ViewBag.UpcomingAppointments = testAppointments;

            return View();
        }
    }
}



