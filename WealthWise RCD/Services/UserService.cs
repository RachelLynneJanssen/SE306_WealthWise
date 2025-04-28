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
            List<Appointment> userAppts = new List<Appointment>();
            if (await _userManager.IsInRoleAsync(user, "User"))
            {
                userAppts = _context.Appointments.Where(a => a.User == user).ToList();
            }
            else if (await _userManager.IsInRoleAsync(user, "Advisor"))
            {
                userAppts = _context.Appointments.Where(a => a.Advisor == user).ToList();
            }
            else    // Admin
            {
                userAppts = _context.Appointments.ToList();
            }
            foreach (Appointment appointment in userAppts)
            {
                appointment.User = await _userManager.FindByIdAsync(appointment.UserId);
                appointment.Advisor = await _userManager.FindByIdAsync(appointment.AdvisorId);
            }
            return userAppts;
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
        public void RemoveAppointment(int id)
        {
            Appointment dbAppt = _context.Appointments.Find(id)!;
            _context.Appointments.Remove(dbAppt);
            _context.SaveChanges();
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
        public void RemovePaymentMethod(int id)
        {
            Payment method = _context.Payments.Find(id)!;
            _context.Payments.Remove(method);
            _context.SaveChanges();
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
                TimeSpan startTime = appointment.ScheduledTime.TimeOfDay;
                TimeSpan endTime = appointment.EndTime;
                if(startTime < new TimeSpan(8, 0, 0) || endTime > new TimeSpan(17,0,0))
                {
                    isAvail = false; // out of range
                }
                else if(startTime > new TimeSpan(12,0,0) && startTime < new TimeSpan(13, 0, 0))
                {
                    isAvail = false; // out of range
                }
                else if (endTime > new TimeSpan(12, 0, 0) && endTime < new TimeSpan(13, 0, 0))
                {
                    isAvail = false; // out of range
                }
                // check within exception
                List<AvailabilityException> potentialConflicts = _context.AvailabilityExceptions.Where(
                                                                    a => (a.AdvisorId == advisor.Id) && (a.Date.Date == appointment.ScheduledTime.Date)
                                                                    ).ToList();
                foreach (AvailabilityException a in potentialConflicts)
                {

                }
            }
            return isAvail;
        }
    }
}