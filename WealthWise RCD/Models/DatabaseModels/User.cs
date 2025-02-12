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
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Age { get; set; }
        public Gender Gender { get; set; }
        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        // public string ProfileLoc { get; set; }
        [Required]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
        public int? BudgetId { get; set; }
        public MonthlyBudget? Budget { get; set; }
        public ICollection<AdvisorEvent>? RegisteredEvents { get; set; }     // Renamed to avoid confusion
        public string? Biography { get; set; }
        // public string ImageLoc { get; set; }
        public ICollection<Certificate>? Certificates { get; set; }
        public ICollection<AdvisorEvent>? AdvisorEvents { get; set; }
    }
}
