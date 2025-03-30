using Microsoft.AspNetCore.Identity;
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

        public async Task<List<Appointment>> GetAllAppointmentsAsync(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            
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
                var updatedAddress = await _context.Appointments.FindAsync(appt.Id);
                if (updatedAddress != null)
                {
                    updatedAddress.DateTime = appt.DateTime;
                    updatedAddress.AdvisorId = appt.AdvisorId;
                    updatedAddress.Advisor = appt.Advisor;
                    updatedAddress.UserId = appt.UserId;
                    updatedAddress.User = appt.User;
                }
            }
            await _context.SaveChangesAsync();
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
    }
}