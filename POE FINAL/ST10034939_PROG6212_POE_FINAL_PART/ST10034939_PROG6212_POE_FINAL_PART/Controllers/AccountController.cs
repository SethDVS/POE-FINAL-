using Microsoft.AspNetCore.Mvc;
using ST10034939_PROG6212_POE_FINAL_PART.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ST10034939_PROG6212_POE_FINAL_PART.Controllers;


namespace ST10034939_PROG6212_POE_FINAL_PART.Controllers
{
    public class AccountController : Controller
    {
        private string connectionString = @"Server=tcp:prog6212part2.database.windows.net,1433;Initial Catalog=poepart2prog;Persist Security Info=False;User ID=ST10034939;Password=Fordpalm36;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = HashPassword(model.Password);

                if (model.SelectedOption == "Register")
                {
                    // Registration logic
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@Username", model.Username);
                            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Redirect or notify user of successful registration
                }
                else if (model.SelectedOption == "Login")
                {
                    // Login logic
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@Username", model.Username);
                            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                            int userCount = (int)cmd.ExecuteScalar();

                            if (userCount == 1)
                            {
                                // User credentials are valid, now retrieve UserId
                                string getUserIdSql = "SELECT UserId FROM Users WHERE Username = @Username";
                                using (SqlCommand getUserIdCmd = new SqlCommand(getUserIdSql, connection))
                                {
                                    getUserIdCmd.Parameters.AddWithValue("@Username", model.Username);
                                    int userId = (int)getUserIdCmd.ExecuteScalar();

                                    // Set the global CurrentUserId
                                    GlobalVariables.CurrentUserId = userId;


                                    // Login successful, use the userId as needed
                                    ViewBag.Message = "Login successful.";

                                    // Example: Store userId in ViewBag or pass it to another method
                                    ViewBag.UserId = userId;
                                }
                            }
                            else
                            {
                                // Invalid credentials
                                ViewBag.Message = "Invalid credentials. Please try again.";
                                // Notify user or handle error
                            }
                        }

                    }
                }
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
