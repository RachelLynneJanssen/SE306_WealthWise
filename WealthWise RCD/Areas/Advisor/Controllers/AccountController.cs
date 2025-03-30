using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models.DatabaseModels;
using WealthWise_RCD.Models;
using WealthWise_RCD.Services;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    [Authorize(Roles = "Advisor,Admin")]
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private string CurrentUserId;
        public AccountController(UserService userService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userService = userService;
            _userManager = userManager;
            _context = context;
            CurrentUserId = _userManager.GetUserId(User)!;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadProfilePartial()
        {
            ApplicationUser currentUser = await _userManager.FindByIdAsync(CurrentUserId);
            return PartialView("Account/_ProfilePartial", currentUser);
        }
        public async Task<IActionResult> LoadAppointmentsPartial()
        {
            List<Appointment> userAppointments = await _userService.GetAllAppointmentsAsync(CurrentUserId);
            return PartialView("Account/_AppointmentsPartial", userAppointments);
        }
        public IActionResult LoadBlogPostsPartial()
        {
            return PartialView("Account/_BlogPostsPartial");
        }
        // await _userManager.UpdateAsync(user);
    }
}
