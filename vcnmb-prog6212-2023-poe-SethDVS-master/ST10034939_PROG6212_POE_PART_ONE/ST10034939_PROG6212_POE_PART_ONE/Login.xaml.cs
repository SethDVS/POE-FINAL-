using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Core;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ST10034939_PROG6212_POE_PART_ONE
{
    public partial class Login : Window
    {

        // Connection string property
        private string connectionString = @"Server=tcp:prog6212part2.database.windows.net,1433;Initial Catalog=poepart2prog;Persist Security Info=False;User ID=ST10034939;Password=Fordpalm36;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public Login()
        {
            InitializeComponent();
        }

        // Add the hashing method here
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

        private void AddModule_Click(object sender, RoutedEventArgs e)
        {
          
            string selectedOption = (loginOrRegisterCombo.SelectedItem as ComboBoxItem).Content.ToString();

            if (selectedOption == "Register")
            {
                // Registration logic=
                string hashedPassword = HashPassword(txtPassword.Text);

                // Store the hashed password and username in the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)", connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else if (selectedOption == "Login")
            {
                // Login logic =
                string hashedPassword = HashPassword(txtPassword.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash", connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        int userCount = (int)cmd.ExecuteScalar();

                        if (userCount == 1)
                        {
                            // Retrieve UserId
                            SqlCommand getUserIdCmd = new SqlCommand("SELECT UserId FROM Users WHERE Username = @Username", connection);
                            getUserIdCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                            int userId = (int)getUserIdCmd.ExecuteScalar();

                            MessageBox.Show("Login Successful!");
                            MainWindow main = new MainWindow(userId);
                            main.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid credentials!");
                        }
                    }
                }
            }

        }

    }
}
