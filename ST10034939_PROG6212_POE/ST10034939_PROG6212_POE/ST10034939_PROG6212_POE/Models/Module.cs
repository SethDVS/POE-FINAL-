using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10034939_PROG6212_POE.Models
{
    public class Module
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid number of credits")]
        public int Credits { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number of class hours per week")]
        public double ClassHoursPerWeek { get; set; }
        
        [NotMapped]
        public int TotalWeeks { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid number of weeks")]
        public int WeeksInSemester { get; set; }

        // This property might be calculated based on other values
        public double SelfStudyHours { get; set; }

        // Additional methods can be added here as needed, for example:
        public void CalculateSelfStudyHours(int totalWeeks)
        {
            if (totalWeeks > 0)
            {
                SelfStudyHours = ((Credits * 10) / (double)totalWeeks) - ClassHoursPerWeek;
            }
            else
            {
                SelfStudyHours = 0;
            }
        }
    }

    public class ModuleViewModel
    {
        public Module NewModule { get; set; }
        public List<Module> ModuleList { get; set; }
    }
    

}
