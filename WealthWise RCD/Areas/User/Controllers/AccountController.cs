﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;
using WealthWise_RCD.Services;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]
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
        public IActionResult LoadAppointmentsPartial()
        {
            return PartialView("Account/_AppointmentsPartial");
        }
        public IActionResult LoadSubscriptionPartial()
        {
            return PartialView("Account/_SubscriptionPartial");
        }
        public IActionResult LoadPaymentMethodsPartial()
        {
            return PartialView("Account/_PaymentMethodsPartial");
        }
        // var user = _userService.GetUserAsync(User);
        // await _userManager.UpdateAsync(user);
    }
}
