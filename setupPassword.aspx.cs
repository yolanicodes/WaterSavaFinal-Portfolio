using System;
using System.Configuration;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace WaterSavaFinal
{
    public partial class setupPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string createPassword = txtCreatePassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(email))
            {
                lblMessage.Text = "Email address is required.";
                return;
            }
            if (string.IsNullOrEmpty(createPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblMessage.Text = "Both password fields are required.";
                return;
            }
            if (createPassword != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            // Check if email exists in the Resident table
            if (!EmailExistsInResident(email))
            {
                lblMessage.Text = "Email address not found in the system.";
                return;
            }

            // Save the email and password to the database
            bool isSaved = SavePasswordToDatabase(email, createPassword);

            if (isSaved)
            {
                lblMessage.Text = "Password setup successfully.";
                Response.Redirect("~/Login.aspx");

            }
            else
            {
                lblMessage.Text = "An error occurred while setting up the password.";
            }
        }

        private bool EmailExistsInResident(string email)
        {
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT COUNT(*) FROM [Resident] WHERE [Resident_Username] = @email";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(CS))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("email", email);

                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return false;
            }
        }

        private bool SavePasswordToDatabase(string email, string password)
        {
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Ensure you hash the password before storing it in the database
            string hashedPassword = HashPassword(password); // Implement HashPassword method according to your hashing strategy

            string query = "UPDATE [User] SET [User_Password] = ? WHERE [User_Email] = ?";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(CS))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Add parameters in the order they appear in the query
                        cmd.Parameters.AddWithValue("?", hashedPassword);
                        cmd.Parameters.AddWithValue("?", email);

                        conn.Open();

                        // Execute the query and check the number of rows affected
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Debugging: Print or log the number of rows affected
                        Console.WriteLine($"Rows affected: {rowsAffected}");

                        // Return true if at least one row was updated
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }



        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
