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
        private ApplicationUser _currentUser;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ApplicationUser user)
        {
            _userManager = userManager;
            _context = context;
            _currentUser = user;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            if (await _userManager.IsInRoleAsync(_currentUser, "User"))
            {
                return _context.Appointments.Where(a => a.User == _currentUser).ToList();
            }
            else if (await _userManager.IsInRoleAsync(_currentUser, "Advisor"))
            {
                return _context.Appointments.Where(a => a.Advisor == _currentUser).ToList();
            }
            else    // Admin
            {
                return _context.Appointments.ToList();
            }
        }


    }
}