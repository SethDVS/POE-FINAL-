using System.Collections.ObjectModel;
using System.Reflection;

namespace TaskManager.Core
{
    public class StudyRecord
    {
        // Reference to the associated module for which the study hours were recorded.
        public Module Module { get; set; }

        // The date on which the study hours were recorded.
        public DateTime Date { get; set; }

        // Number of hours studied on the specified date for the associated module.
        public double HoursStudied { get; set; }
    }

    public class Module
    {
        // Unique code of the module.
        public string Code { get; set; }

        // Name of the module.
        public string Name { get; set; }

        // Number of credits for the module.
        public int Credits { get; set; }

        // Number of hours spent in class for this module each week.
        public double ClassHoursPerWeek { get; set; }

        // Number of self-study hours expected per week for this module.
        public double SelfStudyHours { get; set; }


        /// Calculates the total number of study hours for the semester.
        public int TotalHoursForSemester()
        {
            return Credits * 10;
        }


        /// Calculates the number of self-study hours required for this module per week.
        public double CalculateSelfStudyHours(int totalWeeks)
        {
            if (totalWeeks <= 0)
            {
                return 0; // Return 0 if an invalid number of weeks is provided.
            }

            // Calculate self-study hours based on total hours for semester and class hours.
            return ((Credits * 10) / (double)totalWeeks) - ClassHoursPerWeek;
        }


        /// Calculates the total hours studied for this module during the current week.
        public double HoursStudiedThisWeek(ObservableCollection<StudyRecord> studyRecords)
        {
            // Determine the start and end dates of the current week.
            var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

            // Filter the study records for this module for the current week and sum the hours.
            return studyRecords.Where(s => s.Module == this && s.Date >= startOfWeek && s.Date <= endOfWeek)
                               .Sum(s => s.HoursStudied);
        }


        /// Calculates the number of self-study hours remaining for this module in the current week.
        public double HoursRemainingThisWeek(ObservableCollection<StudyRecord> studyRecords)
        {
            // Get the number of hours already studied this week.
            var studied = HoursStudiedThisWeek(studyRecords);

            // Return the difference between required self-study hours and hours already studied.
            return SelfStudyHours - studied;
        }
    }
}