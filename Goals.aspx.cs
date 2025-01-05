using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace WaterSavaFinal
{
    public partial class Goals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {if (Session["UserEmail"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            string residentUsername = Session["UserEmail"].ToString();
            

            int userID = GetUserID(residentUsername);
           
                lblNoConsumptionMessage.Visible = false;
                BindGoals();
                lblCurrentDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
                BindGridView();
                LoadActivityData();
                PanelAddGoal.Visible = false;
                grdGoals.Visible = false; // Or the appropriate method for chart binding
            
        }
        protected void btnShowAddGoal_Click(object sender, EventArgs e)
        {
            PanelAddGoal.Visible = true;
            grdGoals.Visible = false;
        }

        // Show Edit Goals Panel with GridView
        protected void btnShowEditGoals_Click(object sender, EventArgs e)
        {
            PanelAddGoal.Visible = false;
            grdGoals.Visible = true;
        }
        private void BindGridView()
        {
            // Call calculateRemainingConsumption to get goal, used, and remaining values
            var consumptionValues = CalculateRemainingConsumption();

            decimal goalValue = consumptionValues.GoalValue;
            decimal actualConsumption = consumptionValues.ActualConsumption;
            decimal remainingConsumption = consumptionValues.RemainingConsumption;

            // Create a DataTable to hold Goal, Used, and Remaining values
            DataTable dt = new DataTable();
            dt.Columns.Add("Goal");
            dt.Columns.Add("Used");
            dt.Columns.Add("Remaining");

            // Create a single row for the DataTable
            DataRow row = dt.NewRow();
            row["Goal"] = $"{goalValue} liters ";
            row["Used"] = $" - {actualConsumption} liters";
            row["Remaining"] = $" = {remainingConsumption} liters";
            dt.Rows.Add(row);

            // Clear existing columns if any
            GridView1.Columns.Clear();

            // Bind DataTable to GridView
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        private void LoadActivityData()
        {
            string residentUsername = Session["UserEmail"].ToString();
            ////String residentUsername = "EsethuMnyombolo@gmail.com";
            //String residentUsername = "testuser@gmail.com";
            int residentID = GetUserID(residentUsername);  // Implement method to get current user’s ID
            DateTime currentDate = DateTime.Today;
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                // Query to fetch activity name, goal, water consumption, and percentage
                string query = @"
            SELECT 
    A.Activity_Description & ' (' & G.Goal_Value & ')' AS Activity,
    G.Goal_Value,
    SUM(IIF(WC.[WC_Metric] = 'ml', WC.[WCQuantityofWater] / 1000.0, WC.[WCQuantityofWater])) AS WaterConsumption,
    ROUND((SUM(IIF(WC.[WC_Metric] = 'ml', WC.[WCQuantityofWater] / 1000.0, WC.[WCQuantityofWater])) / IIF(G.Goal_Value = 0, NULL, G.Goal_Value)) * 100, 2) AS Percentage
FROM 
    (([ResidentActivity] RA
INNER JOIN 
    [WaterConsumption] WC ON RA.[WC_ID] = WC.[WC_ID])
INNER JOIN 
    [Activity] A ON RA.[Activity_Num] = A.[ActivityID])
INNER JOIN 
    [Goals] G ON G.Goal_Activity = A.Activity_Description AND G.User_ID = RA.UserID
WHERE 
    RA.[Resident_Username] = @username
    AND WC.[WCDate] = @date
GROUP BY 
    A.Activity_Description, G.Goal_Value";
;

                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    cmd.Parameters.AddWithValue("@username", residentUsername);
                    cmd.Parameters.AddWithValue("@date", currentDate);

                    dbCon.Open();

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Bind the result to the GridView
                        ActivityGridView.DataSource = dt;
                        ActivityGridView.DataBind();
                    }
                }
            }
        }
        // Method to bind the GridView with goals data
        private void BindGoals()
        {
            string residentUsername = Session["UserEmail"].ToString();

            int userID = GetUserID(residentUsername);
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                string query = "SELECT [Goal_ID], [Goal_Description], [Goal_Value], [Goal_Date],[Goal_Activity] FROM [Goals] WHERE [User_ID] = @UserID";
                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    dbCon.Open();

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        grdGoals.DataSource = dt;
                        grdGoals.DataBind();
                    }
                }
            }
        }

        private (decimal GoalValue,decimal ActualConsumption, decimal RemainingConsumption) CalculateRemainingConsumption()
        {
            string residentUsername = Session["UserEmail"].ToString();
            ////String residentUsername = "EsethuMnyombolo@gmail.com";
            //String residentUsername = "testuser@gmail.com";

            if (string.IsNullOrEmpty(residentUsername))
            {
                lblRemainingConsumption.Text = "User not logged in.";
                
            }

            // Variables to hold the goal and actual consumption values
            decimal goalValue = 0;
            decimal actualConsumption = 0;

            // Connection string to the Access database
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                dbCon.Open();

                // Query to get the overall consumption goal for the resident
                string goalQuery = "SELECT [Goal_Value] FROM [Goals] WHERE [Resident_Username] = @username AND Goal_Activity = 'Personal Consumption'";
                using (OleDbCommand goalCmd = new OleDbCommand(goalQuery, dbCon))
                {
                    goalCmd.Parameters.AddWithValue("@username", residentUsername);
                    object result = goalCmd.ExecuteScalar();
                    if (result != null)
                    {
                        goalValue = Convert.ToDecimal(result);
                    }
                }

                // Query to get the actual consumption for the current day for the resident
                string consumptionQuery = @"
        SELECT 
            SUM(IIF(WC.[WC_Metric] = 'ml', WC.[WCQuantityofWater] / 1000.0, WC.[WCQuantityofWater])) AS TotalConsumption
        FROM ([ResidentActivity] RA
        INNER JOIN [WaterConsumption] WC ON RA.[WC_ID] = WC.[WC_ID])
        INNER JOIN [Activity] A ON RA.[Activity_Num] = A.[ActivityID]
        WHERE RA.[Resident_Username] = @username 
        AND RA.[ResidentActivity_StartDate] = @date";

                using (OleDbCommand consumptionCmd = new OleDbCommand(consumptionQuery, dbCon))
                {
                    consumptionCmd.Parameters.AddWithValue("@username", residentUsername);
                    consumptionCmd.Parameters.AddWithValue("@date", DateTime.Now.Date);

                    object result = consumptionCmd.ExecuteScalar();
                    if (result == DBNull.Value)
                    {
                        actualConsumption = 0;
                       
                    }
                    else
                    {
                        // Handle the case when the result is DBNull, for example, set to a default value
                        actualConsumption = Convert.ToDecimal(result); // or any default value you want to assign
                    }
                }
            }

            // Calculate remaining consumption and update the label
            decimal remainingConsumption = goalValue - actualConsumption;
           
            return (goalValue,actualConsumption,remainingConsumption);
        }
    
// Method to get the UserID based on the Resident Username
private int GetUserID(string username)
        {
            int userID = 0;
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                string query = "SELECT [UserID] FROM [Resident] WHERE [Resident_Username] = @Username";
                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    dbCon.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        userID = Convert.ToInt32(result);
                    }
                }
            }
            return userID;
        }

        // Edit row event
        protected void grdGoals_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdGoals.EditIndex = e.NewEditIndex;
            BindGoals();
        }

        // Cancel edit event
        protected void grdGoals_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdGoals.EditIndex = -1;
            BindGoals();
        }

        // Update row event
        protected void grdGoals_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grdGoals.Rows[e.RowIndex];

            // Get the Goal ID
            int goalID = Convert.ToInt32(grdGoals.DataKeys[e.RowIndex].Values[0]);

            // Find controls
            TextBox txtGoalDescription = (TextBox)row.FindControl("txtGoalDescription");
            TextBox txtGoalValue = (TextBox)row.FindControl("txtGoalValue");
            TextBox txtGoalDate = (TextBox)row.FindControl("txtGoalDate");
            HiddenField hfActivityID = (HiddenField)row.FindControl("hfActivityID");

            // Get Activity ID from the hidden field
          //  int activityID = Convert.ToInt32(hfActivityID.Value); // Retrieve the Activity_ID

            // Validate input
            if (!int.TryParse(txtGoalValue.Text, out int goalValue))
            {
                lblMessage.Text = "Invalid goal value.";
                return;
            }
            if (!DateTime.TryParse(txtGoalDate.Text, out DateTime goalDate))
            {
                lblMessage.Text = "Invalid goal date.";
                return;
            }

            string description = txtGoalDescription.Text;

            // Connection string and query setup
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                // Update only Goal_Description, Goal_Value, and Goal_Date; keep the Activity_ID intact
                string query = "UPDATE [Goals] SET [Goal_Description] = @Description, [Goal_Value] = @Value, [Goal_Date] = @Date WHERE [Goal_ID] = @GoalID";

                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    // Add parameters
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Value", goalValue);
                    cmd.Parameters.AddWithValue("@Date", goalDate);
                    cmd.Parameters.AddWithValue("@GoalID", goalID);

                    try
                    {
                        dbCon.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error updating goal: " + ex.Message;
                    }
                }
            }

            // Exit edit mode and refresh data
            grdGoals.EditIndex = -1;
            BindGoals();
            Response.Redirect(Request.RawUrl);
        }



        // Delete row event
        protected void grdGoals_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int goalID = Convert.ToInt32(grdGoals.DataKeys[e.RowIndex].Values[0]);

            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                string query = "DELETE FROM [Goals] WHERE [Goal_ID] = @GoalID";
                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    cmd.Parameters.AddWithValue("@GoalID", goalID);
                    dbCon.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            BindGoals();
        }
        // Add new goal method (as you already have)
        protected void btnAddGoals_Click(object sender, EventArgs e)
        {
            PanelAddGoal.Visible = true;
            string description = txtDescription.Text;
            int capacity = Convert.ToInt32(txtCapacity.Text);
            DateTime date = Convert.ToDateTime(txtDate.Text);
            //string residentUsername = Session["UserEmail"].ToString();
            //String residentUsername = "EsethuMnyombolo@gmail.com";
            String residentUsername = "testuser@gmail.com"; // Example username
            int userID = GetUserID(residentUsername); // Function to retrieve the user ID

            string activity;
            if (ddlGoalType.SelectedValue == "OverallConsumption")
            {
                activity = "Personal Consumption"; // Set a default value for overall consumption
            }
            else
            {
                activity = activities.SelectedValue; // Get the selected activity
            }

            // Check if a goal for the same activity already exists
            if (ddlGoalType.SelectedValue == "Activity" && GoalExists(userID, activity))
            {
                lblMessage.Text = "You already have a goal set for this activity.";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                string query = "INSERT INTO [Goals] ([Goal_Description], [Goal_Value], [Goal_Date], [User_ID], [Resident_Username], [Goal_Activity]) " +
                               "VALUES (@Description, @Value, @Date, @UserID, @Username, @Activity)";

                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Value", capacity);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Username", residentUsername);
                    cmd.Parameters.AddWithValue("@Activity", activity); // Use the selected activity

                    dbCon.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result == 1)
                    {
                        lblMessage.Text = "Goal added successfully!";
                    }
                    else
                    {
                        lblMessage.Text = "Problem with adding the goal.";
                    }
                }
            }

            BindGoals(); // Refresh the GridView
        }


        // Method to check if the goal already exists for the specified user and activity
        // Method to check if the user has already set a goal for the specified activity
        private bool GoalExists(int userID, string activity)
        {
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                string query = "SELECT COUNT(*) FROM [Goals] WHERE [User_ID] = ? AND [Goal_Activity] = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    // Add parameters without '@'
                    cmd.Parameters.AddWithValue("UserID", userID);
                    cmd.Parameters.AddWithValue("Activity", activity);
                    dbCon.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0; // Returns true if a goal exists for the user and activity
                }
            }
        }

        protected void ddlGoalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure the panel remains visible
            PanelAddGoal.Visible = true;

            if (ddlGoalType.SelectedValue == "Activity")
            {
                activities.Visible = true; // Show activities dropdown
                PopulateActivities(); // Fill activities dropdown
                txtCapacity.Visible = false; // Hide capacity textbox
            }
            else if (ddlGoalType.SelectedValue == "OverallConsumption")
            {
                activities.Visible = false; // Hide activities dropdown
                txtCapacity.Visible = true; // Show capacity textbox
            }
            else
            {
                activities.Visible = false;
                txtCapacity.Visible = false;
            }
        }
        private int GetPropertyIdForResident(string email)
        {
            int propertyId = 0;

            // Here, implement your logic to get the PropertyID from the Resident table based on the email
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString)) 
            { 
                dbConnection.Open();
                string query = "SELECT [PropertyID] FROM [Resident] WHERE [Resident_Username] = ?";

                using (var command = new OleDbCommand(query, dbConnection))
                {
                    command.Parameters.AddWithValue("?", email);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        propertyId = Convert.ToInt32(result);
                    }
                }
            }

            return propertyId;
        }
        private void PopulateActivities()
        {
            string residentUsername = Session["UserEmail"].ToString();
            //String residentUsername = "EsethuMnyombolo@gmail.com";
            //String username = "testuser@gmail.com";// Example username
            //String username = "EsethuMnyombolo@gmail.com";
            int propertyId = GetPropertyIdForResident(residentUsername);
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();
                string query = @"SELECT a.[Activity_Description] 
                         FROM [Activity] a
                         INNER JOIN [Property] p ON a.[Property_ID] = p.[Property_ID]
                         WHERE p.[Property_ID] = ?"; // Use parameterized query

                using (OleDbCommand cmd = new OleDbCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("?", propertyId); // Correct parameter usage
                    try
                    {
                        OleDbDataReader reader = cmd.ExecuteReader();

                        activities.DataSource = reader;
                        activities.DataTextField = "Activity_Description"; // This should match the alias in the query
                        activities.DataValueField = "Activity_Description"; // Optionally, you can keep the same value if needed

                        activities.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                    activities.Items.Insert(0, new ListItem("--Select Activity--", ""));
                }

                dbConnection.Close();
            }
        }

    }
}

