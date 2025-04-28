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


            ViewBag.UserName = user.FirstName + " " + user.LastName;
            ViewBag.UpcomingAppointments = userAppts;
            ViewBag.Advisor = advisors;

            return View();
        }

        [HttpPost]
        public ActionResult CancelAppointment(int appointmentId, ApplicationUser user)
        {
            _userService.RemoveAppointment(appointmentId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDate(int appointmentId, DateOnly appointmentDate, TimeOnly appointmentTime)
        {
            DateTime appointmentDateTime = new(appointmentDate, appointmentTime);
            Appointment? appointment = _context.Appointments.Find(appointmentId)!;

            if (appointment != null)
            {

                if (appointmentDateTime < DateTime.Now)
                {
                    TempData["Error"] = "New appointment date cannot be in the past.";
                    return RedirectToAction("Index");
                }
                appointment.ScheduledTime = appointmentDateTime;
                await _userService.UpsertAppointment(appointment);
            }
            else
            {
                TempData["Error"] = "Appointment not found.";

            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> AddAppointment(DateOnly appointmentDate, TimeOnly appointmentTime, string advisorId)
        {
            DateTime appointmentDateTime = new(appointmentDate, appointmentTime);

            if (string.IsNullOrWhiteSpace(advisorId))
            {
                TempData["Error"] = "Advisor Name is required.";
                return RedirectToAction("Index");
            }
            ApplicationUser? advisor = await _userManager.FindByIdAsync(advisorId);
            //var advisors = await _userManager.GetUsersInRoleAsync("Advisor");
            //ApplicationUser? advisor = advisors.FirstOrDefault(u => u.FirstName == advisorId);
            //var advisor = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == advisorId);

            if (advisor == null)
            {
                TempData["Error"] = "Advisor not found.";
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            Appointment newAppointment = new Appointment
            {
                ScheduledTime = appointmentDateTime,
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



