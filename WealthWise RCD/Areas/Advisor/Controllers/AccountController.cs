﻿using Microsoft.AspNetCore.Authorization;
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

        public AccountController(UserService userService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userService = userService;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadProfilePartial()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var getAddress = _userService.GetAddressAsync(user);
            getAddress.Wait();
            user.Address = getAddress.Result;
            return PartialView("Account/_ProfilePartial", user);
        }
        public async Task<IActionResult> LoadAppointmentsPartial()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            List<Appointment> userAppts = await _userService.GetAllAppointmentsAsync(user);
            return PartialView("Account/_AppointmentsPartial", userAppts);
        }
        public async Task<IActionResult> LoadBlogPostsPartial()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var getBlogPosts = _userService.GetAllAdvisorPostsAsync(user);
            getBlogPosts.Wait();
            List<Blog> blogPosts = getBlogPosts.Result;
            return PartialView("Account/_BlogPostsPartial", blogPosts);
        }

        public async Task<IActionResult> LoadEditProfilePartial()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound(); }

            var getAddress = _userService.GetAddressAsync(user);
            getAddress.Wait();
            user.Address = getAddress.Result;
            return PartialView("Account/_EditProfilePartial", user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationUser model)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound(); }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Address = await _userService.GetAddressAsync(user);

            user.Address.StreetName = model.Address.StreetName;
            user.Address.City = model.Address.City;
            user.Address.State = model.Address.State;
            user.Address.ZipCode = model.Address.ZipCode;

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CancelAppointmentAsync(int id)
        {
            _userService.RemoveAppointment(id);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return RedirectToAction("Index", user);
        }
    }
}
