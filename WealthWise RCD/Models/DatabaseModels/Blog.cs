﻿using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title {  get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public decimal Price {  get; set; }
        public string Topic {  get; set; }
        public string Content {  get; set; }
        public int RecommendationScore { get; set; }

        public string AdvisorId { get; set; }

        [ForeignKey(nameof(AdvisorId))]
        public User Advisor { get; set; } = null!;

    }
}
