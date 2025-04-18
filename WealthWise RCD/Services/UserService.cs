﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync(ApplicationUser user)
        {
            
            if (await _userManager.IsInRoleAsync(user, "User"))
            {
                return _context.Appointments.Where(a => a.User == user).ToList();
            }
            else if (await _userManager.IsInRoleAsync(user, "Advisor"))
            {
                return _context.Appointments.Where(a => a.Advisor == user).ToList();
            }
            else    // Admin
            {
                return _context.Appointments.ToList();
            }
        }
        public async Task UpsertAppointment(Appointment appt)
        {
            if (appt.Id == 0)
            {
                _context.Appointments.Add(appt);
            }
            else
            {
                var updatedAppt = await _context.Appointments.FindAsync(appt.Id);
                if (updatedAppt != null)
                {
                    updatedAppt.ScheduledTime = appt.ScheduledTime;
                    updatedAppt.AdvisorId = appt.AdvisorId;
                    updatedAppt.Advisor = appt.Advisor;
                    updatedAppt.UserId = appt.UserId;
                    updatedAppt.User = appt.User;
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Address> GetAddressAsync(ApplicationUser user)
        {
            return await _context.Addresses.FindAsync(user.AddressId);
        }
        public async Task UpsertAddressAsync(Address address)
        {
            if (address.Id == 0)
            {
                _context.Addresses.Add(address);
            }
            else
            {
                var updatedAddress = await _context.Addresses.FindAsync(address.Id);
                if (updatedAddress != null)
                {
                    updatedAddress.StreetName = address.StreetName;
                    updatedAddress.ExtraInfo = address.ExtraInfo;
                    updatedAddress.City = address.City;
                    updatedAddress.State = address.State;
                    updatedAddress.ZipCode = address.ZipCode;
                }
            }
            await _context.SaveChangesAsync();
        }
        public Task<List<Blog>> GetAllAdvisorPostsAsync(ApplicationUser advisor)
        {
            return _context.BlogPosts.Where(b => b.AdvisorId == advisor.Id).ToListAsync();
        }
        //public async Task<IActionResult> SetAvailability(List<AvailabilitySlot> slots, string advisorId)
        //{

        //    // Remove old slots and add new
        //    var oldSlots = _context.AvailabilitySlots.Where(s => s.AdvisorId == advisorId);
        //    _context.AvailabilitySlots.RemoveRange(oldSlots);

        //    foreach (var slot in slots)
        //    {
        //        slot.AdvisorId = advisorId;
        //    }

        //    await _context.AvailabilitySlots.AddRangeAsync(slots);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Availability");
        //}
    }
}