using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;
using WealthWise_RCD.Migrations;
using Org.BouncyCastle.Security;

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

        // private static list that doesn't get created everytime page refreshes
        // so if anything is changed, you can see the change
        private static List<Appointment> testAppointments = new List<Appointment>
            {
                new Appointment { Id = 1, UserId = "USER1", ScheduledTime = DateTime.Now.AddDays(1), EndTime = TimeSpan.FromDays(2), AdvisorId = "Sivan"},
                new Appointment { Id = 2, UserId = "USER2", ScheduledTime = DateTime.Now.AddDays(2), EndTime = TimeSpan.FromDays(2), AdvisorId = "10001"},
                new Appointment { Id = 3, UserId = "USER3", ScheduledTime = DateTime.Now.AddDays(3), EndTime = TimeSpan.FromDays(2), AdvisorId = "10002" },
                new Appointment { Id = 4, UserId = "USER4", ScheduledTime = DateTime.Now.AddDays(4), EndTime = TimeSpan.FromDays(2), AdvisorId = "10003" }
            };

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

            var appointments = _context.Appointments
                .Where(a => a.UserId == currentUser.Id)
                .OrderBy(a => a.ScheduledTime)
                .ToList();

            // this is for when using DB
            //var appointments = await _context.Appointments
            //    .Where(a => a.UserId == currentUser.Id)
            //    .OrderBy(a => a.EndTime.ToString())
            //    .ToListAsync();

            ViewBag.UserName = currentUser.UserName;
            ViewBag.UpcomingAppointments = testAppointments;
            ViewBag.AdvisorId = currentUser.advisorId;

            return View();
        }

        [HttpPost]

        public IActionResult CancelAppointment (int appointmentId)
        {
            var appointment = testAppointments.FirstOrDefault(a => a.Id == appointmentId);

            if(appointment != null)
            {
                appointment.EndTime.ToString();
                Console.WriteLine($"Cancelled Appointment ID: {appointment.Id}");
                testAppointments.Remove(appointment);
                //_context.Appointments.Remove(appointment);
                //await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeAdvisor(int appointmentId, string newAdvisorId)
        {
            var appointment = testAppointments.FirstOrDefault(a => a.Id == appointmentId);

            if (appointment != null && !string.IsNullOrWhiteSpace(newAdvisorId))
            {
                Console.WriteLine($"Changed Advisor for Appointment {appointment.Id} from {appointment.AdvisorId} to {newAdvisorId}");
                appointment.AdvisorId = newAdvisorId;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeDate(int appointmentId, DateTime newAppointmentTime)
        {
            var appointment = testAppointments.FirstOrDefault(a => a.Id == appointmentId);

            if (appointment != null)
            {
                Console.WriteLine($"Changed date for Appointment {appointment.Id} from {appointment.ScheduledTime} to {newAppointmentTime}");
                appointment.ScheduledTime = newAppointmentTime;
            }

            return RedirectToAction("Index");
        }
    }
}



