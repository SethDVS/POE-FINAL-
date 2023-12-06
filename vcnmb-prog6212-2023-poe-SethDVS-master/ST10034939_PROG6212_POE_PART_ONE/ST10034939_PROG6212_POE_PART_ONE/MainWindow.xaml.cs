using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Core;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ST10034939_PROG6212_POE_PART_ONE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // The number of weeks for the semester
        public int Weeks { get; set; }

        // The start date of the semester
        public DateTime SemesterStartDate { get; set; }

        // Collection of modules for the semester
        public ObservableCollection<Module> Modules { get; set; } = new ObservableCollection<Module>();

        // Collection of study records 
        public ObservableCollection<StudyRecord> StudyRecords { get; set; } = new ObservableCollection<StudyRecord>();

        // Connection string property
        private string connectionString = @"Server=tcp:prog6212part2.database.windows.net,1433;Initial Catalog=poepart2prog;Persist Security Info=False;User ID=ST10034939;Password=Fordpalm36;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        public int ModuleId { get; set; }
        public int CurrentUserId { get; set; }
        public MainWindow(int userId)
        {
            InitializeComponent();
            CurrentUserId = userId;
            // Set the ModulesList's data source to Modules collection
            ModulesList.ItemsSource = Modules;

            // Set the dropdown menu's data source to Modules collection
            cmbModule.ItemsSource = Modules;
            cmbModule.DisplayMemberPath = "Name";
        }

        // Event handler for adding a module
        // Event handler for adding a module
        private async void AddModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ensure a valid number of weeks is set
                if (Weeks <= 0)
                {
                    MessageBox.Show("Please set a valid number of weeks first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create a new module object
                var module = new Module
                {
                    Code = txtCode.Text,
                    Name = txtName.Text,
                    Credits = int.Parse(txtCredits.Text),
                    ClassHoursPerWeek = double.Parse(txtWeeksSemester.Text)
                };

                // Calculate self-study hours for the module
                module.SelfStudyHours = module.CalculateSelfStudyHours(Weeks);

                // Add module to the Modules collection
                Modules.Add(module);

                // Asynchronously add module to the database
                await Task.Run(() => AddModuleToDatabase(module));

                // Clear textboxes after adding module
                txtCode.Clear();
                txtName.Clear();
                txtCredits.Clear();
                txtWeeksSemester.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // Update self-study hours for all modules
        private void UpdateSelfStudyHours()
        {
            foreach (var module in Modules)
            {
                module.SelfStudyHours = module.CalculateSelfStudyHours(Weeks);
            }
            // Refresh the Modules list view to show the updated self-study hours
            ModulesList.Items.Refresh();
        }

        // Event handler for setting the number of weeks for the semester
        private void SubmitWeeks_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtWeeks.Text, out int weeks))
            {
                Weeks = weeks;
                UpdateSelfStudyHours();
                MessageBox.Show($"You've set the number of weeks to {weeks}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for setting the start date of the semester
        private void SetStartDate_Click(object sender, RoutedEventArgs e)
        {
            var selectedDate = dpStartDate.SelectedDate;
            if (selectedDate.HasValue)
            {
                SemesterStartDate = selectedDate.Value;
                MessageBox.Show($"The semester start date is set to {SemesterStartDate.ToShortDateString()}", "Date Set", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a valid start date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for changing weeks text input
        private void txtWeeks_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtWeeks.Text, out int weeks))
            {
                Weeks = weeks;
            }
            else
            {
                Weeks = 0; // Resetting the weeks if input is invalid
            }
        }

        // Event handler for recording study hours for a module
        private void RecordHours_Click(object sender, RoutedEventArgs e)
        {
            if (cmbModule.SelectedItem is Module selectedModule &&
                studyDate.SelectedDate.HasValue &&
                double.TryParse(txtHoursStudied.Text, out double hoursStudied))
            {
                var record = new StudyRecord
                {
                    Module = selectedModule,
                    Date = studyDate.SelectedDate.Value,
                    HoursStudied = hoursStudied
                };

                // Fetch the ModuleId based on the selected module name
                int moduleId = GetModuleIdFromDatabase(selectedModule.Name);
                if (moduleId == 0)
                {
                    MessageBox.Show("Error fetching ModuleId from database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Add study record to the StudyRecords collection
                StudyRecords.Add(record);

                // Add study record to the database
                AddStudyRecordToDatabase(record, moduleId);  // Pass the moduleId to the method

                MessageBox.Show("Study hours recorded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a module, date, and valid number of hours.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetModuleIdFromDatabase(string moduleName)
        {
            int moduleId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ModuleId FROM Modules WHERE Name = @Name";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", moduleName);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        moduleId = Convert.ToInt32(result);
                    }
                }
            }
            return moduleId;
        }

        private void AddStudyRecordToDatabase(StudyRecord record, int moduleId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO StudyRecords (ModuleId, UserId, Date, HoursStudied) VALUES (@ModuleId, @UserId, @Date, @HoursStudied)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModuleId", moduleId);
                    command.Parameters.AddWithValue("@UserId", CurrentUserId);
                    command.Parameters.AddWithValue("@Date", record.Date);
                    command.Parameters.AddWithValue("@HoursStudied", record.HoursStudied);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Event handler for updating remaining study hours for modules
        private void UpdateRemainingHours_Click(object sender, RoutedEventArgs e)
        {
            var modulesWithRemainingHours = Modules.Select(m => new
            {
                Name = m.Name,
                RemainingHours = m.HoursRemainingThisWeek(StudyRecords)
            }).ToList();

            // Bind the calculated remaining hours to a data grid for display
            RemainingHoursDataGrid.ItemsSource = modulesWithRemainingHours;
        }


        private void AddModuleToDatabase(Module module)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Modules (UserId, Code, Name, Credits, ClassHoursPerWeek, SelfStudyHours) VALUES (@UserId, @Code, @Name, @Credits, @ClassHours, @SelfStudyHours)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", CurrentUserId);
                    command.Parameters.AddWithValue("@Code", module.Code);
                    command.Parameters.AddWithValue("@Name", module.Name);
                    command.Parameters.AddWithValue("@Credits", module.Credits);
                    command.Parameters.AddWithValue("@ClassHours", module.ClassHoursPerWeek);
                    command.Parameters.AddWithValue("@SelfStudyHours", module.SelfStudyHours);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void AddStudyRecordToDatabase(StudyRecord record)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO StudyRecords (ModuleId, UserId, Date, HoursStudied) VALUES (@ModuleId, @UserId, @Date, @HoursStudied)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModuleId", this.ModuleId);
                    command.Parameters.AddWithValue("@UserId", CurrentUserId);
                    command.Parameters.AddWithValue("@Date", record.Date);
                    command.Parameters.AddWithValue("@HoursStudied", record.HoursStudied);

                    command.ExecuteNonQuery();
                }
            }
        }
















    }
}
