using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WealthWise_RCD.Models.DatabaseModels;
using System.Reflection;

namespace WealthWise_RCD.Models
{
    public class TempBlog
    {
        public string Title { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; } 
        public DateTime SavedAt { get; set; }
    }
}