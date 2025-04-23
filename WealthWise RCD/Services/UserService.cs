using Microsoft.AspNetCore.Identity;
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
                    updatedAppt.EndTime = appt.EndTime;
                    updatedAppt.AdvisorId = appt.AdvisorId;
                    updatedAppt.Advisor = appt.Advisor;
                    updatedAppt.UserId = appt.UserId;
                    updatedAppt.User = appt.User;
                }
            }
            await _context.SaveChangesAsync();
        }
        public Task<List<Payment>> GetAllPaymentMethodsAsync(ApplicationUser user)
        {
            return _context.Payments.Where(p => p.UserId == user.Id).ToListAsync();
        }
        public async Task UpsertPaymentMethod(Payment paymentMethod)
        {
            if (paymentMethod.Id == 0)
            {
                _context.Payments.Add(paymentMethod);
            }
            else
            {
                var updatedPayment = await _context.Payments.FindAsync(paymentMethod.Id);
                if (updatedPayment != null)
                {
                    updatedPayment.Name = paymentMethod.Name;
                    switch (paymentMethod.Type)
                    {
                        case PaymentType.CreditCard:
                            updatedPayment.CardNumber = paymentMethod.CardNumber;
                            updatedPayment.CardholderName = paymentMethod.CardholderName;
                            updatedPayment.ExpDate = paymentMethod.ExpDate;
                            updatedPayment.Cvc = paymentMethod.Cvc;
                            break;
                        case PaymentType.PayPal:
                            updatedPayment.AccountName = paymentMethod.AccountName;
                            break;
                    }
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

        public Task<List<Blog>> GetAllTipsPostsAsync(string tipTopic)
        {
            return _context.BlogPosts.Where(b => b.Topic == tipTopic && b.IsTip).ToListAsync();
        }

        private async Task<bool> IsAvailable(Appointment appointment)
        {
            bool isAvail = false;
            ApplicationUser advisor = await _userManager.FindByIdAsync(appointment.AdvisorId);
            if (advisor != null)
            { 
                DayOfWeek day = appointment.ScheduledTime.DayOfWeek;
                TimeSpan startTime = appointment.ScheduledTime.TimeOfDay;
                TimeSpan endTime = appointment.EndTime;
                // check within time
                // check within exception
            }
            return isAvail;
        }
    }
}