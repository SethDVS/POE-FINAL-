using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ST10034939_PROG6212_POE.Models
{
    public class StudyRecord
    {
        [Key]
        public int Id { get; set; }  // Primary key, if not already defined in your original class

        [Required]
        public int ModuleId { get; set; }  // Foreign key to Module

        [ForeignKey("ModuleId")]
        public Module Module { get; set; }  // Navigation property

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number of hours studied")]
        public double HoursStudied { get; set; }

    }
    public class StudyRecordViewModel
    {
        public int SelectedModuleId { get; set; }
        public List<SelectListItem> Modules { get; set; } = new List<SelectListItem>();
        public int HoursStudied { get; set; }
    }

}

