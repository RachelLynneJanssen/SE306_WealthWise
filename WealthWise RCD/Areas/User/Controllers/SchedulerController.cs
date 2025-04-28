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
using WealthWise_RCD.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WealthWise_RCD.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]

    public class SchedulerController : Controller
    {
        // services
        private readonly UserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        // db models
        private readonly ApplicationUser applicationUser;
        private readonly Appointment appointments;

        public SchedulerController(UserService userSerivce, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userService = userSerivce;
            _userManager = userManager;
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            // check if user is actually logged in

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // stores current user info
            var user = await _userManager.GetUserAsync(User);
            List<Appointment> userAppts = await _userService.GetAllAppointmentsAsync(user);
            //var displayAppt = _context.Appointments
            //    .Where(a => a.UserId == user.Id)
            //    .OrderBy(a => a.EndTime)
            //    .Select(a => new
            //    {
            //        a.Id,
            //        a.ScheduledTime,
            //        a.Advisor.FirstName
            //    })
            //    .ToList();

            var advisors = await _userManager.GetUsersInRoleAsync("Advisor");
                //_context.Roles.Where(u => u.Id == "Advisor").ToListAsync();


            ViewBag.UserName = user.FirstName;
            ViewBag.UpcomingAppointments = userAppts;
            ViewBag.Advisor = advisors;

            return View();
        }

        [HttpPost]
        public ActionResult CancelAppointment(int appointmentId, ApplicationUser user)
        {
            _userService.RemoveAppointment(appointmentId);
            //var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);

            //if (appointment != null)
            //{
            //    _context.Appointments.Remove(appointment);
            //    await _context.SaveChangesAsync();
            //}

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAdvisor(int appointmentId, string newAdvisor)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment != null && !string.IsNullOrWhiteSpace(newAdvisor))
            {
                var advisor = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == newAdvisor);

                if (advisor == null)
                {
                    Console.WriteLine($"Advisor with First Name {newAdvisor} does not exist!");
                    TempData["Error"] = "The selected Advisor does not exist.";
                    return RedirectToAction("Index");
                }

                Console.WriteLine($"Changed Advisor for Appointment {appointment.Id} from {appointment.AdvisorId} to {newAdvisor}");
                appointment.AdvisorId = advisor.Id;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDate(int appointmentId, DateTime newAppointmentTime)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment != null)
            {

                if (newAppointmentTime < DateTime.Now)
                {
                    TempData["Error"] = "New appointment date cannot be in the past.";
                    return RedirectToAction("Index");
                }

                Console.WriteLine($"Changed date for Appointment {appointment.Id} from {appointment.ScheduledTime} to {newAppointmentTime}");
                appointment.ScheduledTime = newAppointmentTime;
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["Error"] = "Appointment not found.";

            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> AddAppointment(DateTime appointmentDate, string advisorName)
        {
            if (string.IsNullOrWhiteSpace(advisorName))
            {
                TempData["Error"] = "Advisor Name is required.";
                return RedirectToAction("Index");
            }
            var advisors = await _userManager.GetUsersInRoleAsync("Advisor");
            ApplicationUser? advisor = advisors.FirstOrDefault(u => u.FirstName == advisorName);
            //var advisor = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == advisorName);

            if (advisor == null)
            {
                TempData["Error"] = "Advisor not found.";
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            Appointment newAppointment = new Appointment
            {
                ScheduledTime = appointmentDate,
                EndTime = TimeSpan.FromMinutes(60),
                AdvisorId = advisor.Id,
                Advisor = advisor,
                UserId = currentUser!.Id,
                User = currentUser!
            };
            bool isTimeSlotTaken = await _userService.UpsertAppointment(newAppointment);
            //bool isTimeSlotTaken = await _context.Appointments.AnyAsync(a => a.ScheduledTime == appointmentDate);

            if (isTimeSlotTaken)
            {
                TempData["Error"] = "Another appointment already exists at this time. Please choose a different time.";
                return RedirectToAction("Index");
            }
            //_context.Appointments.Add(newAppointment);

            //await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}



