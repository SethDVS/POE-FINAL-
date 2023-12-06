using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10034939_PROG6212_POE.Models;
using ST10034939_PROG6212_POE_PART_ONE;
using System.Data.SqlClient;
using System.Text;

namespace ST10034939_PROG6212_POE.Controllers
{
    public class ModulesController : Controller
    {
        private string connectionString = @"Server=tcp:prog6212part2.database.windows.net,1433;Initial Catalog=poepart2prog;Persist Security Info=False;User ID=ST10034939;Password=Fordpalm36;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private List<Module> GetModules()
        {
            var modules = new List<Module>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Modules"; // Modify the SQL query as needed
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var module = new Module
                            {
                                // Assuming your Module model has these properties
                                // Assign them from the reader based on your table's columns
                                Code = reader["Code"].ToString(),
                                Name = reader["Name"].ToString(),
                                Credits = (int)reader["Credits"],
                                ClassHoursPerWeek = (double)reader["ClassHoursPerWeek"],
                                SelfStudyHours = (double)reader["SelfStudyHours"],
                                // Add other properties as needed
                            };
                            modules.Add(module);
                        }
                    }
                }
            }
            return modules;
        }

        // GET: Modules/AddModule
        [HttpGet]
        public IActionResult AddModule()
        {
            return View();
        }

        // GET: Modules/ListModules
        public IActionResult ListModules()
        {
            var modules = GetModules(); // Fetch the list of modules from the database
            return View(modules);
        }


        // POST: Modules/AddModule
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddModule(Module module)
        {
            if (ModelState.IsValid)
            {
                // Check if the CurrentUserId is set
                if (GlobalVariables.CurrentUserId == 0)
                {
                    // The user is not logged in or the ID is not set
                    return RedirectToAction("Login", "Account"); // Redirect to login page
                }

                // Perform the calculation for SelfStudyHours before saving
                module.CalculateSelfStudyHours(module.TotalWeeks);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Modules (UserId, Code, Name, Credits, ClassHoursPerWeek, SelfStudyHours) VALUES (@UserId, @Code, @Name, @Credits, @ClassHoursPerWeek, @SelfStudyHours)";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", GlobalVariables.CurrentUserId); // Use the CurrentUserId from the GlobalVariables
                        cmd.Parameters.AddWithValue("@Code", module.Code);
                        // Removed the duplicate parameter for "Code"
                        cmd.Parameters.AddWithValue("@Name", module.Name);
                        cmd.Parameters.AddWithValue("@Credits", module.Credits);
                        cmd.Parameters.AddWithValue("@ClassHoursPerWeek", module.ClassHoursPerWeek);
                        cmd.Parameters.AddWithValue("@SelfStudyHours", module.SelfStudyHours);

                        cmd.ExecuteNonQuery();
                    }

                    string selectSql = "SELECT ModuleId FROM Modules WHERE UserId = @UserId";
                    using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@UserId", GlobalVariables.CurrentUserId);

                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            GlobalVariables.ModuleIds.Clear(); // Clear the existing items in the list
                            while (reader.Read())
                            {
                                GlobalVariables.ModuleIds.Add(reader.GetInt32(0)); // Assuming ModuleId is the first column
                            }
                        }
                    }
                }

                // Redirect to a confirmation page or module list
                return RedirectToAction("ListModules"); // Replace 'Index' with the appropriate action
            }

            return View(module);
        }

        public IActionResult RecordStudyHours()
        {
            var viewModel = new StudyRecordViewModel
            {
                Modules = GetModulesSelectList()
            };
            return View(viewModel);
        }

        private List<SelectListItem> GetModulesSelectList()
        {
            var modulesSelectList = new List<SelectListItem>();
            var modules = GetModules();
            foreach (var module in modules)
            {
                modulesSelectList.Add(new SelectListItem { Text = module.Name, Value = module.Code });
            }
            return modulesSelectList;
        }


        [HttpPost]
        public IActionResult RecordStudyHours(StudyRecordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SaveStudyRecord(viewModel);
                return RedirectToAction("Index"); // Or wherever you want to redirect after recording the hours
            }
            viewModel.Modules = GetModulesSelectList(); // Repopulate the list if there's a validation error
            return View(viewModel);
        }

            private void SaveStudyRecord(StudyRecordViewModel viewModel)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "INSERT INTO StudyRecords (ModuleId, UserId, Date, HoursStudied) VALUES (@ModuleId, @UserId, @Date, @HoursStudied)";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ModuleId", viewModel.SelectedModuleId);
                        command.Parameters.AddWithValue("@UserId", GlobalVariables.CurrentUserId); // Use GlobalVariables.CurrentUserId
                        command.Parameters.AddWithValue("@Date", DateTime.Now); // Adjust as needed
                        command.Parameters.AddWithValue("@HoursStudied", viewModel.HoursStudied);

                        command.ExecuteNonQuery();
                    }
                }
            }

        }


    }
