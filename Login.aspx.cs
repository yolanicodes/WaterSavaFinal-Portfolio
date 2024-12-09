

using System;
using System.Configuration;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace WaterSavaFinal
{
    public partial class Login : System.Web.UI.Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
           
           if (!IsPostBack)
           {
               SetLoginFieldsEnabled(false);

                HttpCookie authCookie = Request.Cookies["RememberMe"];

                if(authCookie!=null&& !String.IsNullOrEmpty(authCookie.Value))
                {
                    String[] cookieData = authCookie.Value.Split('|');

                    if (cookieData.Length == 2)
                    {
                        String userEmail = cookieData[0];   
                        String userRole = cookieData[1];

                        Session["UserEmail"] = userEmail;

                       
                        if (userRole.Equals("Admin"))
                        {
                            Response.Redirect("indexAdmin.aspx");
                        }
                        else
                        {
                            Response.Redirect("indexResidents.aspx");
                        }
                    }
                   
                }
           }
        }
        protected void UserRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            SetLoginFieldsEnabled(true);
        }
    

        private void SetLoginFieldsEnabled(bool enabled)
        {
           
            var userNameTextBox = (TextBox)LoginID.FindControl("UserName");
            var passwordTextBox = (TextBox)LoginID.FindControl("Password");

            if (userNameTextBox != null && passwordTextBox != null)
            {
                userNameTextBox.Enabled = enabled;
                passwordTextBox.Enabled = enabled;
            }
        }

        protected void Login_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string username = LoginID.UserName;
            string password = LoginID.Password;

            // Hash the entered password
            string hashedPassword = HashPassword(password);

            // Validate the user with the hashed password
            bool isValid = ValidateUser(username, hashedPassword);

            // Log the hashed password for debugging
            lblMessage.Text = $"Entered Hashed Password: {hashedPassword}";

            if (isValid)
            {
                e.Authenticated = true;
                Session["UserEmail"] = username;
                Session["LoginTime"]= DateTime
                    .Now;

               
                ContentPlaceHolder contentPlaceHolder = (ContentPlaceHolder)Master.FindControl("FeaturedContent");
                if (contentPlaceHolder == null)
                {
                    lblMessage.Text = "ContentPlaceHolder not found.";
                    e.Authenticated = false;
                    return;
                }

                RadioButtonList userRoleControls = (RadioButtonList)contentPlaceHolder.FindControl("userRoles");
                if (userRoleControls == null)
                {
                    lblMessage.Text = "Role selection control not found.";
                    e.Authenticated = false;
                    return;
                }

                string userRole = userRoleControls.SelectedValue;

                if (((CheckBox)LoginID.FindControl("RememberMe")).Checked)
                {
                    HttpCookie rememberMeCookie = new HttpCookie("RememberMe", $"{username}|{userRole}")
                    {
                        Expires = DateTime.Now.AddDays(30)
                    };
                    Response.Cookies.Add(rememberMeCookie); 
                }

                switch (userRole)
                {
                    case "Admin":
                        Response.Redirect("indexAdmin.aspx", true);
                        Session["LoginTime"] = DateTime.Now;
                        break;
                    case "Resident":
                        Response.Redirect("indexResidents.aspx", true);
                        Session["LoginTime"] = DateTime.Now;
                        break;
                    default:
                        Response.Redirect("NewUser.aspx", true);
                        break;
                }
            }
            else
            {
                lblMessage.Text = "Invalid username or password!";
                e.Authenticated = false;
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
        private bool ValidateUser(string username, string hashedPassword)
        {
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            bool isValid = false;

            using (OleDbConnection dbCon = new OleDbConnection(CS))
            {
                string sqlCommand = "SELECT [User_Password] FROM [User] WHERE [User_Email] = @Email";

                using (OleDbCommand cmd = new OleDbCommand(sqlCommand, dbCon))
                {
                    cmd.Parameters.AddWithValue("@Email", username);

                    dbCon.Open();
                    object result = cmd.ExecuteScalar();
                    dbCon.Close();

                    // Check if a result was returned
                    if (result != null)
                    {
                        string storedHashedPassword = result.ToString();
                        if (storedHashedPassword == hashedPassword)
                        {
                            isValid = true;
                        }
                    }
                }
            }

            return isValid;
        }
        private int GetUserIdFromEmail(string email)
        {
            int userId = -1;
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT User_ID FROM [User] WHERE User_Email = email";

         OleDbConnection conn = new OleDbConnection(CS);
                OleDbCommand cmd = new OleDbCommand(query, conn);
                
                    // Add the parameter with the email value
                    cmd.Parameters.AddWithValue("?", email);
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            userId = Convert.ToInt32(result);
                        }
                    conn.Close();
                 
            return userId;
        }

    }
}