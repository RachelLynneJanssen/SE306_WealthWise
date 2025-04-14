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
        public async Task<IActionResult> LoadPaymentMethodsPartial()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var getPaymentMethods = _userService.GetAllPaymentMethodsAsync(user);
            getPaymentMethods.Wait();
            List<Payment> paymentMethods = getPaymentMethods.Result;

            return PartialView("Account/_PaymentMethodsPartial", paymentMethods);
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

        public async Task<IActionResult> LoadEditPaymentMethodPartial(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound(); }

            var paymentMethod = await _context.Payments.FindAsync(id);
            if (paymentMethod == null || paymentMethod.UserId != user.Id)
            {
                return NotFound();
            }

            return PartialView("Account/_EditPaymentMethodPartial", paymentMethod);
        }

        public async Task<IActionResult> LoadAddPaymentMethodPartial()
        {
            var payment = new Payment();
            return PartialView("Account/_AddPaymentMethodPartial");
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
            user.Address.ExtraInfo = model.Address.ExtraInfo;
            user.Address.City = model.Address.City;
            user.Address.State = model.Address.State;
            user.Address.ZipCode = model.Address.ZipCode;

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentMethod(Payment model)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound(); }
            //var paymentMethod = await _context.Payments.FindAsync(model.Id);
            //if (paymentMethod == null || paymentMethod.UserId != user.Id) { return NotFound(); }
            //paymentMethod.Type = model.Type;
            //paymentMethod.Name = model.Name;
            //paymentMethod.UserId = user.Id;
            //paymentMethod.User = user;
            

            //if (model.Type == PaymentType.CreditCard)
            //{
            //    paymentMethod.CardNumber = model.CardNumber;
            //    paymentMethod.CardholderName = model.CardholderName;
            //    paymentMethod.ExpDate = model.ExpDate;
            //    paymentMethod.Cvc = model.Cvc;
            //}
            //else if (model.Type == PaymentType.PayPal)
            //{
            //    paymentMethod.AccountName = model.AccountName;
            //}
            model.UserId = user.Id;
            model.User = user;

            await _userManager.UpdateAsync(user);
            await _userService.UpsertPaymentMethod(model);

            return RedirectToAction("Index", user);
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentMethod(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return View("Account/_EditPaymentMethodPartial", payment);
            }

            // Set user and save logic here
            var user = await _userManager.GetUserAsync(User);
            payment.UserId = user.Id;

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction("PaymentMethods"); // or however you want to handle the redirect
        }

    }
}
