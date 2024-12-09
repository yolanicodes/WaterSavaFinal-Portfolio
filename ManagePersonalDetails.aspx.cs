using System;
using System.Configuration;
using System.Data.OleDb;
using System.Web.UI;

namespace WaterSavaFinal
{
    public partial class ManagePersonalDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserEmail"] != null)
                {
                    string userEmail = Session["UserEmail"].ToString();
                    LoadNameAndRole();
                    LoadUserDetails(userEmail);
                    pnlEditDetails.Visible = false;
                }
                else
                {
                    // Handle the case where the session might be null or expired
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void LoadUserDetails(string email)
        {
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(CS))
            {
                string sqlCommand = "SELECT [User_LName], [User_FName], [User_Title], [User_ContactNo], [User_Email] FROM [User] WHERE [User_Email] = @email";

                using (OleDbCommand cmd = new OleDbCommand(sqlCommand, dbCon))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    dbCon.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblFirstName.Text = reader["User_FName"].ToString();
                            lblLastName.Text = reader["User_LName"].ToString();
                            lblTitle.Text = reader["User_Title"].ToString();
                            lblContactNumber.Text = reader["User_ContactNo"].ToString();
                            lblEmailAddress.Text = reader["User_Email"].ToString();

                            txtFirstName.Text = lblFirstName.Text;
                            txtLastName.Text = lblLastName.Text;
                            txtTitle.Text = lblTitle.Text;
                            txtContactNumber.Text = lblContactNumber.Text;
                            txtEmailAddress.Text = lblEmailAddress.Text;
                        }
                    }
                }
            }
        }

        private void LoadNameAndRole()
        {
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(CS))
            {
                string email = Session["UserEmail"].ToString();
                string sqlCommand = "SELECT [User_FName], [User_LName], [User_Role] FROM [User] WHERE [User_Email] = @email";

                using (OleDbCommand cmd = new OleDbCommand(sqlCommand, dbCon))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    dbCon.Open(); 
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader["User_FName"].ToString();
                            string lastName = reader["User_LName"].ToString();
                            string userRole = reader["User_Role"].ToString();
                            h2ProfileUser.InnerText = $"{firstName[0]}. {lastName[0]}.";
                            h3UserRole.InnerText = userRole;
                            
                        }
                        else
                        {
                            lblDebug.Text = "User details not found.";
                        }
                    }
                }
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            pnlEditDetails.Visible = true;
            if (pnlEditDetails.Visible == true)
            {
        lblDebugging.Text = "Edit button clicked. Displaying Panel";
            }
            else
            {
                lblDebugging.Text = "Edit panel cannot display. Error occured";
            }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(CS))
            {
                string sqlCommand = "UPDATE [User] SET [User_FName] = @fname, [User_LName] = @lname, [User_Title] = @title, [User_ContactNo] = @contact WHERE [User_Email] = @email";

                using (OleDbCommand cmd = new OleDbCommand(sqlCommand, dbCon))
                {
                    cmd.Parameters.AddWithValue("@fname", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@lname", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@contact", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmailAddress.Text);

                    dbCon.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Reload details and hide edit panel
            LoadUserDetails(Session["UserEmail"].ToString());
            pnlEditDetails.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEditDetails.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(CS))
            {
                string sqlCommand = "DELETE FROM [User] WHERE [User_Email] = ?";

                using (OleDbCommand cmd = new OleDbCommand(sqlCommand, dbCon))
                {
                    cmd.Parameters.AddWithValue("?", Session["UserEmail"].ToString());

                    dbCon.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Redirect to the login page or another page after deletion
            Response.Redirect("Login.aspx");
        }
    }
}
