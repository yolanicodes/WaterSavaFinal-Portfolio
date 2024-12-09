using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WaterSavaFinal
{
    public partial class NewAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Hide the property section on page load
                PropertyPanel.Visible = false;
                ResidentPanel.Visible = false;  
                AdminPanel.Visible = true;
            }
        }
        protected void Save_Admin_Details(object sender, EventArgs e)
        {
            String pword = HashPassword(CreatePassword.Text);
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon=new OleDbConnection(cs);
            string sqlcommand1 = "INSERT into [User] ([User_LName], [User_FName], [User_Email], [User_ContactNo], [User_Title], [User_Password],[User_Role]) VALUES (@LastName, @firstName ,@Email, @Contact,@Title, @Password, @Role)";
            OleDbCommand cmd1 = new OleDbCommand(sqlcommand1, dbCon);
            cmd1.Parameters.AddWithValue("@firstName", firstName.Text);
            cmd1.Parameters.AddWithValue("@LastName",LastName.Text);
            cmd1.Parameters.AddWithValue("@Email", Email.Text);
            cmd1.Parameters.AddWithValue("@Contact", int.Parse(Contact.Text));
            cmd1.Parameters.AddWithValue("@Title", getSelectedTitle());
            cmd1.Parameters.AddWithValue("@Password",pword);
            cmd1.Parameters.AddWithValue("@Role", "Admin");
            dbCon.Open();
            int ReturnCode1=cmd1.ExecuteNonQuery();
            dbCon.Close();


            if (ReturnCode1==1)
            {
                Session["AdminEmail"]=Email.Text;   
                lblMessgae.Text = "You are now added";
                PropertyPanel.Visible = true;
                AdminPanel.Visible = false;
                FNameValidator.Enabled = true;
                LNameValidator.Enabled = true;
                emailValidator.Enabled = false;
                ContactValidator.Enabled = false;
                CreatePasswordValidator.Enabled = false;
                ConfirmPasswordValidator.Enabled = false;
            }
            else
            {
                lblMessgae.Text = "Problem with adding";
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
        private int CountAdminProperties(int adminUserID)
        {
            int propertyCount = 0;
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                try
                {
                    dbCon.Open();
                    // SQL query to count properties associated with the admin's UserID
                    string query = "SELECT COUNT(*) FROM [AdminProperty] WHERE UserID = @AdminUserID";
                    OleDbCommand cmd = new OleDbCommand(query, dbCon);
                    // Add parameter for the admin's UserID
                    cmd.Parameters.AddWithValue("@AdminUserID", adminUserID);
                    // Execute the query and get the count of properties
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        propertyCount = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception (log it or display error message)
                    lblMessgae.Text = "Error: " + ex.Message;
                }
                finally
                {
                    dbCon.Close();
                }
            }

            return propertyCount;
        }

        protected void Save_Property_Details(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);
            
            string sqlcommand2 = "INSERT INTO [Property] ([Property_HouseNum], [Property_StreetName], [Property_Suburb], [Property_City], [Property_Province], [Property_PostalCode]) VALUES (@HouseNum, @StreetName, @Suburb, @City, @Province, @PostalCode)";
            OleDbCommand cmd2 = new OleDbCommand(sqlcommand2, dbCon);
                
                    cmd2.Parameters.AddWithValue("@HouseNum", int.Parse(houseNo.Text));
                    cmd2.Parameters.AddWithValue("@StreetName", houseStreet.Text);
                    cmd2.Parameters.AddWithValue("@Suburb", houseSub.Text);
                    cmd2.Parameters.AddWithValue("@City", houseCity.Text);
                    cmd2.Parameters.AddWithValue("@Province", houseProv.SelectedValue);
                    cmd2.Parameters.AddWithValue("@PostalCode",int.Parse(houseCode.Text));
                        dbCon.Open();
                        int ReturnCode2 = cmd2.ExecuteNonQuery();
                        dbCon.Close();
                        if (ReturnCode2 == 1)
                        {
                             Save_Admin_Info();
                             lblMessgae.Text = "Property details saved successfully.";
                             PropertyPanel.Visible = false;
                             ResidentPanel.Visible = true; 
                        }
                        else
                        {
                            lblMessgae.Text = "Problem with saving property details.";
                        }
                 
                        lblMessgae.Visible = true;
        }
        private void Save_Admin_Info()
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);
            
                dbCon.Open();

            String adminEmail = Session["AdminEmail"].ToString();

            int AdminUserID = GetLastInsertedAdminId(adminEmail, dbCon);
            int noOfProperties = CountAdminProperties(AdminUserID);
                // Insert Admin record into Admin table
                string insertAdminSql = "INSERT INTO [Admin] ([User_ID], [No_of_Properties]) VALUES (@UserId, @NoOfProperties)";
                OleDbCommand insertAdminCmd = new OleDbCommand(insertAdminSql, dbCon);
                insertAdminCmd.Parameters.AddWithValue("@UserId", AdminUserID); // Assuming you have a method to get the last inserted User ID
            insertAdminCmd.Parameters.AddWithValue("@NoOfProperties",noOfProperties);
               int returnCode1= insertAdminCmd.ExecuteNonQuery();

                // Insert record into association table between Admin and Property
                string associateAdminPropertySql = "INSERT INTO [AdminProperty] ([UserID], [PropertyID]) VALUES (@AdminId, @PropertyId)";
                OleDbCommand associateAdminPropertyCmd = new OleDbCommand(associateAdminPropertySql, dbCon);
                associateAdminPropertyCmd.Parameters.AddWithValue("@AdminId", GetLastInsertedAdminId(adminEmail,dbCon)); 
                associateAdminPropertyCmd.Parameters.AddWithValue("@PropertyId", GetLastInsertedPropertyId(dbCon)); 
               int returnCode2= associateAdminPropertyCmd.ExecuteNonQuery();
           
            dbCon.Close();

            if (returnCode1 == 1 && returnCode2 == 1)
            {
                lblMessgae.Text = "admin Information stored successfully";
            }
            else
            {
                throw new Exception(lblMessgae.Text = "Error storing admin info");
            }
            
        }
        protected void Save_Resident_Info(object sender, EventArgs e)
        {
            // Example logic to save resident information
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);
            
                string sqlcommand = "INSERT INTO [User] ([User_LName], [User_FName], [User_Email], [User_ContactNo], [User_Title], [User_Role]) VALUES (@LastName,@FirstName, @Email, @Contact, @Title, @Role)";
                OleDbCommand cmd = new OleDbCommand(sqlcommand, dbCon);

                cmd.Parameters.AddWithValue("@FirstName", Textbox1.Text);
                cmd.Parameters.AddWithValue("@LastName", Textbox2.Text);
                cmd.Parameters.AddWithValue("@Email", Textbox3.Text);
                cmd.Parameters.AddWithValue("@Contact", int.Parse(Textbox4.Text));
                cmd.Parameters.AddWithValue("@Title", getSelectedTitle());
                cmd.Parameters.AddWithValue("@Role", "Resident");
                dbCon.Open();
                int returnCode = cmd.ExecuteNonQuery();
                dbCon.Close();

            if (returnCode == 1)
            {
                InsertResidentRecord(Textbox3.Text);
                lblMessgae.Visible = true;
                ClearResidentFields();
                lblMessgae.Text = "Resident information saved successfully.";

                string emailRecipient=Textbox3.Text.Trim();

                string subject = " Setup your WaterSAVA Account";

                string body = "Hey " + Textbox2.Text+","+ "\n\n Please click the link below to set up your password:\n\n\" +\r\n                  \"file:///C:/Users/mbali/OneDrive/Desktop/OneDrive/WaterSava/WaterSavaFinal/setupPassword.aspx";

                string mailto="mailto:"+emailRecipient + "?subject=" + Uri.EscapeDataString(subject) +
                        "&body=" + Uri.EscapeDataString(body);
                Response.Redirect(mailto);
            }
            else
            {
                lblMessgae.Text = "Problem with saving resident information.";
            }

           
            
        }
        protected void SaveFinal_Resident_Info(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);

            string sqlcommand = "INSERT INTO [User] ([User_LName], [User_FName], [User_Email], [User_ContactNo], [User_Title], [User_Role]) VALUES (@LastName,@FirstName, @Email, @Contact, @Title, @Role)";
            OleDbCommand cmd = new OleDbCommand(sqlcommand, dbCon);

            cmd.Parameters.AddWithValue("@FirstName", Textbox1.Text);
            cmd.Parameters.AddWithValue("@LastName", Textbox2.Text);
            cmd.Parameters.AddWithValue("@Email", Textbox3.Text);
            cmd.Parameters.AddWithValue("@Contact", int.Parse(Textbox4.Text));
            cmd.Parameters.AddWithValue("@Title", getSelectedTitle());
            cmd.Parameters.AddWithValue("@Role", "Resident");
            dbCon.Open();
            int returnCode = cmd.ExecuteNonQuery();
            dbCon.Close();


                if (returnCode == 1)
                {
                InsertResidentRecord(Textbox3.Text);
                lblMessgae.Text = "Resident information saved successfully. Redirecting to Home page";
                    LoadAdminData();
                    LoadPropertyData();
                LoadResidentData();
                string emailRecipient = Textbox3.Text.Trim();

                string subject = " Setup your WaterSAVA Account";

                string body = "Hey " + Textbox2.Text + "," + "\n\n Please click the link below to set up your password:\n\n\" +\r\n                  \"file:///C:/Users/mbali/OneDrive/Desktop/OneDrive/WaterSava/WaterSavaFinal/setupPassword.aspx";

                string mailto = "mailto:" + emailRecipient + "?subject=" + Uri.EscapeDataString(subject) +
                        "&body=" + Uri.EscapeDataString(body);
                Response.Redirect(mailto);
                Response.Redirect("indexAdmin.aspx");
            }
            else
                {
                    lblMessgae.Text = "Problem with saving resident information in Resident table.";
                }
            

            lblMessgae.Visible = true;
           
        }
        
        private void LoadAdminData()
        {
            string email = Session["AdminEmail"].ToString();
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);
            int User_ID = GetLastInsertedAdminId(email,dbCon);
            string sqlquery = "SELECT * FROM [User] WHERE [User_ID]=@User_ID";
            OleDbCommand cmd = new OleDbCommand(sqlquery, dbCon);
            cmd.Parameters.AddWithValue("@User_ID", User_ID); // Ensure you have the correct User_ID
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                gvAdmins.DataSource = dt;
                gvAdmins.DataBind();
                gvAdmins.Visible=true;
            }
            else
            {
                lblMessgae.Text = "No admin data found.";
            }
        }
        private void LoadPropertyData()
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);
            int PropertyId = GetLastInsertedPropertyId(dbCon);
            string sqlquery = "SELECT * FROM [Property] WHERE [Property_ID] = @PropertyId";
            OleDbCommand cmd = new OleDbCommand(sqlquery, dbCon);
            cmd.Parameters.AddWithValue("@PropertyId", PropertyId);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                gvProperties.DataSource = dt;
                gvProperties.DataBind();
                gvProperties.Visible=true;
            }
            else
            {
                lblMessgae.Text = "No admin data found.";
            }
        }
        private void LoadResidentData()
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);
            dbCon.Open();
            // Step 1: Get the last inserted Property_ID
            int lastPropertyID = GetLastInsertedPropertyId(dbCon); // Assuming GetLastInsertedPropertyId() method is already defined
            // Step 2: Get all residents associated with this Property_ID
            string queryResidents = "SELECT UserID FROM [Resident] WHERE PropertyID = @lastPropertyID";
            OleDbCommand cmdResidents = new OleDbCommand(queryResidents, dbCon);
            cmdResidents.Parameters.AddWithValue("@lastPropertyID", lastPropertyID);
            OleDbDataAdapter daResidents = new OleDbDataAdapter(cmdResidents);
            DataTable dtResidents = new DataTable();
            daResidents.Fill(dtResidents);

            if (dtResidents.Rows.Count > 0)
            {
                // Step 3: For each resident, retrieve their details from the User table
                string residentUserIDs = string.Join(",", dtResidents.AsEnumerable().Select(row => row["UserID"].ToString()));
                string queryUsers = $"SELECT * FROM [User] WHERE User_ID IN ({residentUserIDs})";
                OleDbDataAdapter daUsers = new OleDbDataAdapter(queryUsers, dbCon);
                DataTable dtUsers = new DataTable();
                daUsers.Fill(dtUsers);
                // Bind the results to your GridView
                gvResidents.DataSource = dtUsers;
                gvResidents.DataBind();
                gvResidents.Visible = true;
            }
            dbCon.Close();
        }
        private void ClearResidentFields()
        {
            Textbox1.Text = "";
            Textbox2.Text = "";
            Textbox3.Text = "";
            Textbox4.Text = "";
            rbtnMiss.Checked=false;
            rbtnMiss.Checked = false;
            rbtnMiss.Checked = false;
            rbtnMiss.Checked = false;
        }

        private String getSelectedTitle()
        {
            if (rbtnMiss.Checked)
            {
                return "Miss";
            }
            if (rbtnMr.Checked)
            {
                return "Mr";
            }
            if (rbtnMs.Checked)
            {
                return "Ms";
            }
            if (rbtnMrs.Checked)
            {
                return "Mrs";
            }
            return null;
        }
        private int GetUserIdByEmail(string email)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                dbCon.Open();

                string query = "SELECT User_ID FROM [User] WHERE [User_Email] = @Email";
                OleDbCommand cmd = new OleDbCommand(query, dbCon);
                cmd.Parameters.AddWithValue("@Email", email);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("User not found with email: " + email);
                }
            }
        }

        private int GetLastInsertedAdminId(string email, OleDbConnection dbCon)
        {
            string query = "SELECT [User_ID] FROM [User] WHERE [User_Email] = @Email";
            OleDbCommand cmd = new OleDbCommand(query, dbCon);
            cmd.Parameters.AddWithValue("@Email", email);



            if (dbCon.State == ConnectionState.Closed)
            {
                dbCon.Open();
            }
            object result= cmd.ExecuteScalar();

            

            if (result != DBNull.Value && result != null)
            {
                lblMessgae.Text = "Admin found successfully!";
                return Convert.ToInt32(result);
            }
            else
            {
                lblMessgae.Text = "Error finding admin!";
                throw new Exception(lblMessgae.Text);
            }
        }
        // Method to get the last inserted Property ID
        private int GetLastInsertedPropertyId(OleDbConnection dbCon)
        {
            string query = "SELECT MAX(Property_ID) FROM [Property]";
            OleDbCommand cmd = new OleDbCommand(query, dbCon);

            if (dbCon.State == ConnectionState.Closed)//to open database
            {
                dbCon.Open();
            }
            object result = cmd.ExecuteScalar();
            if (result != DBNull.Value && result != null)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("Failed to retrieve the last inserted User ID.");
            }
        }

        protected void Clear_Click(object sender, EventArgs e)
        {

        }
        private void InsertResidentRecord(string residentEmail)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                dbCon.Open();

                // Get the User_ID of the resident using their email
                int userID = GetUserIdByEmail(residentEmail);

                // Get the last inserted PropertyID
                int propertyID = GetLastInsertedPropertyId(dbCon);

                // Insert into the Resident table
                string sqlResidentInsert = "INSERT INTO [Resident] ([Resident_Username], [UserID], [PropertyID]) VALUES (@Username, @UserID, @PropertyID)";
                OleDbCommand cmdResident = new OleDbCommand(sqlResidentInsert, dbCon);

                cmdResident.Parameters.AddWithValue("@Username", residentEmail);  // Use email as Resident_Username
                cmdResident.Parameters.AddWithValue("@UserID", userID);  // User_ID from the User table
                cmdResident.Parameters.AddWithValue("@PropertyID", propertyID);  // Last inserted property
               

                cmdResident.ExecuteNonQuery();

                dbCon.Close();
            }
        }

    }
}