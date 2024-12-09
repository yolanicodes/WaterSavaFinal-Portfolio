using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace WaterSavaFinal
{
    public partial class ManageProperties : System.Web.UI.Page
    {
        string userEmail;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserEmail"] != null)
                {
                    userEmail = Session["UserEmail"].ToString();
                    PropertyPanel.Visible = false;
                    ResidentPanel.Visible = false;
                    activityPanel.Visible = false;
                    LoadProperties();
                  
                }
                else
                {
                    // Handle the case where the session might be null or expired
                    Response.Redirect("Login.aspx");
                }
            }
        }
        private void BindGrid()
        {
            // Fetch data from the database
            DataTable dt = GetPropertiesData(); // Replace with your actual data retrieval method

            if (dt != null && dt.Rows.Count > 0)
            {
                listProperties.DataSource = dt;
                listProperties.DataBind();
            }
            else
            {
                // Optional: Handle the case when there are no records
                listProperties.DataSource = null;
                listProperties.DataBind();
            }
        }
        private DataTable GetPropertiesData()
        {
            DataTable dt = new DataTable();

            string username = "Faithjob@gmail.com";
            //string username = userEmail;  // Ensure this variable holds the correct email value
            string query = @"
    SELECT p.[Property_ID], p.[Property_HouseNum], p.[Property_StreetName], 
           p.[Property_Suburb], p.[Property_City], p.[Property_Province], 
           p.[Property_PostalCode]
    FROM ([Property] p
    INNER JOIN [AdminProperty] ap ON p.[Property_ID] = ap.[PropertyID])
    INNER JOIN [User] u ON ap.[UserID] = u.[User_ID]
    WHERE u.[User_Email] = @Email AND p.[Property_InUse] = True";

            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection connection = new OleDbConnection(CS))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@Email", username);

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
               

                try
                {
                    connection.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listProperties.DataSource = dt;
                        listProperties.DataBind();
                    }
                    else
                    {
                        lblDebug.Text = "No properties found for the current admin.";
                    }
                }
                catch (Exception ex)
                {
                    lblDebug.Text = "An error occurred: " + ex.Message;
                }
            }
            return dt;
        }

        private void LoadProperties()
        {
            //string username = "Faithjob@gmail.com";
            string username = userEmail;  // Ensure this variable holds the correct email value
            string query = @"
    SELECT p.[Property_ID], p.[Property_HouseNum], p.[Property_StreetName], 
           p.[Property_Suburb], p.[Property_City], p.[Property_Province], 
           p.[Property_PostalCode]
    FROM ([Property] p
    INNER JOIN [AdminProperty] ap ON p.[Property_ID] = ap.[PropertyID])
    INNER JOIN [User] u ON ap.[UserID] = u.[User_ID]
    WHERE u.[User_Email] = @Email AND p.[Property_InUse] = True"; 

            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection connection = new OleDbConnection(CS))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@Email", username);

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable propertiesTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(propertiesTable);

                    if (propertiesTable.Rows.Count > 0)
                    {
                        listProperties.DataSource = propertiesTable;
                        listProperties.DataBind();
                    }
                    else
                    {
                        lblDebug.Text = "No properties found for the current admin.";
                    }
                }
                catch (Exception ex)
                {
                    lblDebug.Text = "An error occurred: " + ex.Message;
                }
            }
        }

        protected void listProperties_RowEditing(object sender, GridViewEditEventArgs e)
        {
            listProperties.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void listProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listProperties.SelectedDataKey != null)
            {
                // Get the selected property ID
                int selectedPropertyId = (int)listProperties.SelectedDataKey.Value;

                // Display the Property Details Panel
                propertyDetailsPanel.Visible = true;

                string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (OleDbConnection dbCon = new OleDbConnection(cs))
                {
                    dbCon.Open();

                    string query = "SELECT u.[User_FName], u.[User_LName] FROM [Resident] r INNER JOIN [User] u ON r.[Resident_Username] = u.[User_Email] WHERE r.[PropertyID] = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                    {
                        cmd.Parameters.AddWithValue("?", selectedPropertyId);

                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable residentsTable = new DataTable();
                        adapter.Fill(residentsTable);

                        // Bind the DataTable to the GridView
                        gvResidents.DataSource = residentsTable;
                        gvResidents.DataBind();
                    }

                    dbCon.Close();
                }

                using (OleDbConnection dbCon = new OleDbConnection(cs))
                {
                    dbCon.Open();
                    string activityQuery = "SELECT * FROM [Activity] WHERE [Property_ID] = ?";
                    using (OleDbCommand command = new OleDbCommand(activityQuery, dbCon))
                    {
                        command.Parameters.AddWithValue("?", selectedPropertyId);
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                        DataTable activitiesTable = new DataTable();
                        adapter.Fill(activitiesTable);

                        // Bind the DataTable to the GridView
                        gvActivities.DataSource = activitiesTable;
                        gvActivities.DataBind();
                    }
                    dbCon.Close();  
                }

                // Debugging output
                lblDebug.Text = "Selected Property ID: " + selectedPropertyId;
            }
       
}


        protected void listProperties_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (listProperties.Rows[e.RowIndex].RowType == DataControlRowType.DataRow)
            {
                GridViewRow row = listProperties.Rows[e.RowIndex];
                int propertyID = Convert.ToInt32(listProperties.DataKeys[e.RowIndex].Value.ToString());

                string txtHouseNum = e.NewValues["Property_HouseNum"].ToString();
                string txtStreetName = e.NewValues["Property_StreetName"].ToString();
                string txtSuburb = e.NewValues["Property_Suburb"].ToString();
                string txtCity = e.NewValues["Property_City"].ToString();
                DropDownList ddlProvince = (DropDownList)row.FindControl("ddlProvince");
                string txtPostalCode = e.NewValues["Property_PostalCode"].ToString();


                string updateQuery = @"
            UPDATE [Property] 
            SET [Property_HouseNum] = @HouseNum,
                [Property_StreetName] = @StreetName,
                [Property_Suburb] = @Suburb,
                [Property_City] = @City,
                [Property_Province] = @Province,
                [Property_PostalCode] = @PostalCode
            WHERE [Property_ID] = @PropertyID";
                string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (OleDbConnection connection = new OleDbConnection(CS))
                {
                    OleDbCommand command = new OleDbCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@HouseNum", int.Parse(txtHouseNum));
                    command.Parameters.AddWithValue("@StreetName", txtStreetName);
                    command.Parameters.AddWithValue("@Suburb", txtSuburb);
                    command.Parameters.AddWithValue("@City", txtCity);
                    command.Parameters.AddWithValue("Province", ddlProvince.SelectedValue);
                    command.Parameters.AddWithValue("@PostalCode", int.Parse(txtPostalCode));
                    command.Parameters.AddWithValue("@PropertyID", propertyID);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        listProperties.EditIndex = -1;
                        LoadProperties();
                    }
                    catch (Exception ex)
                    {
                        lblDebug.Text = "An error occurred: " + ex.Message;
                    }
                }
            }
        }

        protected void listProperties_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string propertyID = listProperties.DataKeys[e.RowIndex].Value.ToString();
            int userID = GetUserIdByEmail(userEmail);
            // SQL command to uncheck the property in the Property_InUse column
            string updateQuery = @"UPDATE [Property] SET [Property_InUse] = false WHERE [Property_ID] = @PropertyID";
            string deleteAdminPropertyQuery = @"DELETE FROM [AdminProperty] WHERE [PropertyID] = @PropertyID AND [UserID]= @UserID";

            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection connection = new OleDbConnection(CS))
            {
                OleDbCommand command = new OleDbCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@PropertyID", propertyID);

                OleDbCommand deleteAdminPropertyCmd = new OleDbCommand(deleteAdminPropertyQuery, connection);
                deleteAdminPropertyCmd.Parameters.AddWithValue("@PropertyID", propertyID);
                deleteAdminPropertyCmd.Parameters.AddWithValue("@UserID", userID);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    //Updates no of properties
                    string adminEmail = Session["UserEmail"].ToString();
                    int adminUserID = GetUserIdByEmail(adminEmail);

                    string updatePropertyCountSql = "UPDATE [Admin] SET [no_of_properties] = [no_of_properties] - 1 WHERE [User_ID] = @AdminId";
                    OleDbCommand updatePropertyCountCmd = new OleDbCommand(updatePropertyCountSql, connection);
                    updatePropertyCountCmd.Parameters.AddWithValue("@AdminId", adminUserID);
                    updatePropertyCountCmd.ExecuteNonQuery();
                    Response.Redirect(Request.RawUrl);
                    LoadProperties();
                }
                catch (Exception ex)
                {
                    lblDebug.Text = "An error occurred: " + ex.Message;
                }
            }
        }

        protected void btnSave_Property_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);

            string sqlcommand2 = "INSERT INTO [Property] ([Property_HouseNum], [Property_StreetName], [Property_Suburb], [Property_City], [Property_Province], [Property_PostalCode], [Property_InUse]) VALUES (@HouseNum, @StreetName, @Suburb, @City, @Province, @PostalCode,@InUse)";
            OleDbCommand cmd2 = new OleDbCommand(sqlcommand2, dbCon);

            cmd2.Parameters.AddWithValue("@HouseNum", int.Parse(houseNo.Text));
            cmd2.Parameters.AddWithValue("@StreetName", houseStreet.Text);
            cmd2.Parameters.AddWithValue("@Suburb", houseSub.Text);
            cmd2.Parameters.AddWithValue("@City", houseCity.Text);
            cmd2.Parameters.AddWithValue("@Province", ddlProvince.SelectedValue);
            cmd2.Parameters.AddWithValue("@PostalCode", int.Parse(houseCode.Text));
            cmd2.Parameters.AddWithValue("@InUse", chkInUse.Checked);
            dbCon.Open();
            int ReturnCode2 = cmd2.ExecuteNonQuery();
            string adminEmail = Session["UserEmail"].ToString();
            int adminUserID = GetUserIdByEmail(adminEmail);

            string updatePropertyCountSql = "UPDATE [Admin] SET [no_of_properties] = [no_of_properties] + 1 WHERE [User_ID] = @AdminId";
            OleDbCommand updatePropertyCountCmd = new OleDbCommand(updatePropertyCountSql, dbCon);
            updatePropertyCountCmd.Parameters.AddWithValue("@AdminId", adminUserID);
            updatePropertyCountCmd.ExecuteNonQuery();
            dbCon.Close();
            if (ReturnCode2 == 1)
            {
                Save_Admin_Info();
                lblDebug.Text = "Property details saved successfully.";
                
                PropertyPanel.Visible = false;
                Response.Redirect(Request.RawUrl);//ResidentPanel.Visible = true;
            }
            else
            {
                lblDebug.Text = "Problem with saving property details.";
            }

            lblDebug.Visible = true;
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
        private void ClearResidentFields()
        {
            firstName.Text = "";
            LastName.Text = "";
            Email.Text = "";
            Contact.Text = "";
            rbtnMiss.Checked = false;
            rbtnMiss.Checked = false;
            rbtnMiss.Checked = false;
            rbtnMiss.Checked = false;
        }
        private void Save_Admin_Info()
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);

            try
            {
                dbCon.Open();
                String adminEmail = Session["UserEmail"].ToString();

                // Get the Admin User ID and property count
                int AdminUserID = GetUserIdByEmail(adminEmail);
               
                // Insert record into association table between Admin and Property
                string associateAdminPropertySql = "INSERT INTO [AdminProperty] ([UserID], [PropertyID]) VALUES (@AdminId, @PropertyId)";
                OleDbCommand associateAdminPropertyCmd = new OleDbCommand(associateAdminPropertySql, dbCon);
                associateAdminPropertyCmd.Parameters.AddWithValue("@AdminId", AdminUserID);
                associateAdminPropertyCmd.Parameters.AddWithValue("@PropertyId", GetLastInsertedPropertyId(dbCon));
                int returnCode2 = associateAdminPropertyCmd.ExecuteNonQuery();

                // Check if both inserts were successful
                if ( returnCode2 == 1)
                {
                    //This is to update the number of properties, but i commented it out 
                    //string updatePropertyCountSql = "UPDATE [Admin] SET [no_of_properties] = [no_of_properties] + 1 WHERE [User_ID] = @AdminId";
                    //OleDbCommand updatePropertyCountCmd = new OleDbCommand(updatePropertyCountSql, dbCon);
                    //updatePropertyCountCmd.Parameters.AddWithValue("@AdminId", AdminUserID);
                    //updatePropertyCountCmd.ExecuteNonQuery();
                    lblDebug.Text = "Admin information stored successfully.";
                }
                else
                {
                    throw new Exception("Error storing admin information.");
                }
            }
            catch (OleDbException ex)
            {
                lblDebug.Text = "Database error: " + ex.Message;
                // Log or handle database-specific errors here
            }
            catch (Exception ex)
            {
                lblDebug.Text = "An error occurred: " + ex.Message;
                // Log or handle any other general errors here
            }
            finally
            {
                // Ensure the connection is closed even if an exception occurs
                if (dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                }
            }
        }
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

        protected void btnAddProperty_Click(object sender, EventArgs e)
        {
            PropertyPanel.Visible = true;
            ResidentPanel.Visible = false;
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
                int propertyID = (int)listProperties.SelectedDataKey.Value;

                // Insert into the Resident table
                string sqlResidentInsert = "INSERT INTO [Resident] ([Resident_Username], [UserID], [PropertyID]) VALUES (@Username, @UserID, @PropertyID)";
                OleDbCommand cmdResident = new OleDbCommand(sqlResidentInsert, dbCon);

                cmdResident.Parameters.AddWithValue("@Username", residentEmail);  // Use email as Resident_Username
                cmdResident.Parameters.AddWithValue("@UserID", userID);  // User_ID from the User table
                cmdResident.Parameters.AddWithValue("@PropertyID", propertyID);  // Last inserted property


                cmdResident.ExecuteNonQuery();
                Response.Redirect(Request.RawUrl);//ResidentPanel.Visible = true;
                dbCon.Close();
            }
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
        protected void btnSaveActivity_Click(object sender, EventArgs e)
        {
            string selectedPropertyID = listProperties.SelectedDataKey.Value.ToString();
            string activityDescription = txtActivityDescription.Text;

            // Check if an activity with the same description already exists
            string checkQuery = "SELECT COUNT(*) FROM [Activity] WHERE [Activity_Description] = @Description";
            using (OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                OleDbCommand checkCmd = new OleDbCommand(checkQuery, connection);
                checkCmd.Parameters.AddWithValue("@Description", activityDescription);
                connection.Open();

                int activityExists = (int)checkCmd.ExecuteScalar();
                if (activityExists > 0)
                {
                    lblError.Text = "Activity with this description already exists. Please change the description.";
                    return;
                }
                else
                {
                    // Insert the new activity
                    string insertActivityQuery = "INSERT INTO [Activity] ([Activity_Description],[Property_ID], [Activity_Capacity], [Activity_Metric]) VALUES (@Description, @PropertyID, @Capacity, @Unit)";
                    OleDbCommand insertCmd = new OleDbCommand(insertActivityQuery, connection);
                    insertCmd.Parameters.AddWithValue("@Description", activityDescription);
                    insertCmd.Parameters.AddWithValue("@PropertyID", selectedPropertyID);
                    insertCmd.Parameters.AddWithValue("@Capacity", txtCapacity.Text);
                    insertCmd.Parameters.AddWithValue("@Unit", ddlUnit.SelectedValue);

                    insertCmd.ExecuteNonQuery();
                    lblDebug.Text = "Activity Addded successfully!";
                    // Clear input fields and hide the panel
                    txtActivityDescription.Text = "";
                    txtCapacity.Text = "";
                    lblError.Text = ""; // Clear any previous error messages
                    Response.Redirect(Request.RawUrl);//ResidentPanel.Visible = true;
                    activityPanel.Visible = false;
                }
            }
           

        }

        protected void btnAddActivity_Click(object sender, EventArgs e)
        {
            activityPanel.Visible = true;
        }

        protected void btnAddResident_Click(object sender, EventArgs e)
        {
            ResidentPanel.Visible = true;
        }

        protected void Save_Resident_Info(object sender, EventArgs e)
        {
            // Example logic to save resident information
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);

            string sqlcommand = "INSERT INTO [User] ([User_LName], [User_FName], [User_Email], [User_ContactNo], [User_Title], [User_Role]) VALUES (@LastName,@FirstName, @Email, @Contact, @Title, @Role)";
            OleDbCommand cmd = new OleDbCommand(sqlcommand, dbCon);

            cmd.Parameters.AddWithValue("@FirstName", firstName.Text);
            cmd.Parameters.AddWithValue("@LastName", LastName.Text);
            cmd.Parameters.AddWithValue("@Email", Email.Text);
            cmd.Parameters.AddWithValue("@Contact", int.Parse(Contact.Text));
            cmd.Parameters.AddWithValue("@Title", getSelectedTitle());
            cmd.Parameters.AddWithValue("@Role", "Resident");
            dbCon.Open();
            int returnCode = cmd.ExecuteNonQuery();
            dbCon.Close();

            if (returnCode == 1)
            {
                InsertResidentRecord(Email.Text);
                lblDebug.Visible = true;
                ClearResidentFields();
                lblDebug.Text = "Resident information saved successfully.";
                
            }
            else
            {
                lblDebug.Text = "Problem with saving resident information.";
            }



        }
        protected void SaveFinal_Resident_Info(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection dbCon = new OleDbConnection(cs);

            string sqlcommand = "INSERT INTO [User] ([User_LName], [User_FName], [User_Email], [User_ContactNo], [User_Title], [User_Role]) VALUES (@LastName,@FirstName, @Email, @Contact, @Title, @Role)";
            OleDbCommand cmd = new OleDbCommand(sqlcommand, dbCon);

            cmd.Parameters.AddWithValue("@FirstName", firstName.Text);
            cmd.Parameters.AddWithValue("@LastName", LastName.Text);
            cmd.Parameters.AddWithValue("@Email", Email.Text);
            cmd.Parameters.AddWithValue("@Contact", int.Parse(Contact.Text));
            cmd.Parameters.AddWithValue("@Title", getSelectedTitle());
            cmd.Parameters.AddWithValue("@Role", "Resident");
            dbCon.Open();
            int returnCode = cmd.ExecuteNonQuery();
            dbCon.Close();


            if (returnCode == 1)
            {
                InsertResidentRecord(Email.Text);
                lblDebug.Text = "Resident information saved successfully." ;
                Response.Redirect(Request.RawUrl);
                ResidentPanel.Visible = false;
            }
            else
            {
                lblDebug.Text = "Problem with saving resident information in Resident table.";
            }


            lblDebug.Visible = true;

        }
    }
}
