﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public enum Gender
    {
        MALE,
        FEMALE,
        OTHER
    }
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Age { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public int? SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; } = null!;
        public ICollection<MonthlyBudget>? Budgets { get; set; }
        public int? advisorId {  get; set; }
        public ICollection<Payment> PaymentMethods { get; set; }
        public ICollection<AdvisorEvent>? RegisteredEvents { get; set; }     // Renamed to avoid confusion
        public string? Biography { get; set; }
        [MaxLength(255)]    
        public string? ImageLoc { get; set; }
        public ICollection<Certificate>? Certificates { get; set; }
        public ICollection<AdvisorEvent>? AdvisorEvents { get; set; }
        public ICollection<Blog>? BlogPosts { get; set; }
        public ICollection<AvailabilitySlot>? AvailabilitySlots { get; set; }
        public ICollection<AvailabilityException>? AvailabilityExceptions { get; set; }
        public ApplicationUser() { }
    }
}
