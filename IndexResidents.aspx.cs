using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting; 

namespace WaterSavaFinal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userEmail = Session["UserEmail"]?.ToString();
                int propertyID = GetUserPropertyID(userEmail);
                // Set the PropertyID parameter for SqlDataSource3
                SqlDataSource3.SelectParameters["PropertyID"].DefaultValue = propertyID.ToString();

                LoadActivities();
                //checkDailyLimit();
              
            }
        }

        public void checkDailyLimit()
        {
            WaterConsumptionService WaterCheckingService=new WaterConsumptionService();

            string currentUser = Session["UserEmail"].ToString();

            if (!currentUser.Equals(null))
            {
                WaterCheckingService.CheckConsAndNotification(currentUser);
            }
        }
        private void LoadChartInfo(DataTable dt)
        {
            Chart1.Series["Series1"].Points.Clear();

            if (dt.Rows.Count > 0 && dt.Columns.Contains("Activity_Description") && dt.Columns.Contains("TotalAmount"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string activityDescription = dr["Activity_Description"].ToString();
                    double totalAmount = Convert.ToDouble(dr["TotalAmount"]);

                    // Manually create a new DataPoint
                    DataPoint point = new DataPoint();
                    point.SetValueXY(activityDescription, totalAmount);

                    // Set the tooltip for the data point
                    point.ToolTip = $"Activity: {activityDescription}\nTotal Consumption: {totalAmount} L";

                    // Add the DataPoint to the series
                    Chart1.Series["Series1"].Points.Add(point);
                }
            }
            else
            {
                lblMessage.Text = "No data available or incorrect column names.";
            }
        }
        private DataTable GetData(OleDbCommand cmd)
        {
            DataTable data = new DataTable();
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                // Open the connection
                dbCon.Open();
                cmd.Connection = dbCon;

                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    da.Fill(data);
                }
            }
            return data;
        }
        private void LoadActivities()
        {
            string currentUser = Session["UserEmail"]?.ToString();
            DateTime today = DateTime.Now.Date;
            if (string.IsNullOrEmpty(currentUser))
            {
                lblMessage.Text = "UserEmail is null or empty.";
                Response.Redirect("Login.aspx");
                return;
            }
            int propertyID = GetUserPropertyID(currentUser);
            string query = @"
    SELECT [A].[Activity_Description], 
    SUM([WC].[WCQuantityofWater] / 1000.0) AS TotalAmount 
    FROM ([ResidentActivity] RA 
    INNER JOIN [WaterConsumption] WC ON [RA].[WC_ID] = [WC].[WC_ID]) 
    INNER JOIN [Activity] A ON [RA].[Activity_Num] = [A].[ActivityID] 
    WHERE [RA].[Resident_Username] = ? AND [RA].[ResidentActivity_StartDate] = ? 
AND [A].[Property_ID] = ?
    GROUP BY [A].[Activity_Description];
    ";

            using (OleDbCommand cmd = new OleDbCommand(query))
            {
                cmd.Parameters.AddWithValue("?", currentUser);
                cmd.Parameters.AddWithValue("?", today);
                // If PropertyID is available, add it as a parameter
                if (propertyID > 0)
                {
                    cmd.Parameters.AddWithValue("?", propertyID);
                }

                try
                {
                    DataTable dt = GetData(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        LoadChartInfo(dt);
                    }
                    else
                    {
                        lblMessage.Text = "No water consumption data found for today.";
                        Chart1.Visible = false;  // Hide the chart if there's no data
                    }
                }
                catch (OleDbException ex)
                {
                    lblMessage.Text = "OleDbException: " + ex.Message + "<br/>" +
                                      "SQL Query: " + query + "<br/>" +
                                      "User: " + currentUser + "<br/>" +
                                      "Date: " + today.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void btnSaveCons_Click(object sender, EventArgs e)
        {
            double Capacity = Convert.ToDouble(txtCapacity.Text);
            DateTime date = DateTime.Now.Date;
            TimeSpan time = DateTime.Now.TimeOfDay;
            String username = Session["UserEmail"].ToString();
            //String username ="EsethuMnyombolo@gmail.com";
           // String username = "testuser@gmail.com";
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                dbCon.Open();
                // Insert water consumption details
                string sqlcommand1 = "INSERT INTO [WaterConsumption] ([WCQuantityofWater], [WCTime], [WCDate]) VALUES (@quantity, @time, @Date)";
                using (OleDbCommand insertCommand = new OleDbCommand(sqlcommand1, dbCon))
                {
                    insertCommand.Parameters.AddWithValue("@quantity", Capacity);
                    insertCommand.Parameters.AddWithValue("@time", time);
                    insertCommand.Parameters.AddWithValue("@Date", date);

                    // Execute the insert command
                    insertCommand.ExecuteNonQuery();

                    // Retrieve the ID of the inserted water consumption record
                    string sqlGetLastId = "SELECT MAX([WC_ID]) FROM [WaterConsumption]";
                    int WC_Id = 0;
                    using (OleDbCommand getIdCommand = new OleDbCommand(sqlGetLastId, dbCon))
                    {
                        object result = getIdCommand.ExecuteScalar();
                        if (result != null)
                        {
                            WC_Id = Convert.ToInt32(result);
                        }
                    }
                    //Insert ResidentActivity table with new water consumption record
                    InsertResidentActivity(dbCon, WC_Id, username);

                    lblMessage.Text = "Resident activity added successfully";

                    dbCon.Close();
                }// Optionally, refresh the page to update the chart
                Response.Redirect(Request.RawUrl);
            }
        }
        private bool ResidentActivityExists(OleDbConnection dbCon, string username, int activityNum, DateTime date)
        {
            string query = @"SELECT COUNT(*) FROM [ResidentActivity] 
                     WHERE [Resident_Username] = @Username 
                     AND [Activity_Num] = @ActivityNum 
                     AND [ResidentActivity_StartDate] = @Date";

            using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@ActivityNum", activityNum);
                cmd.Parameters.AddWithValue("@Date", date);

                try
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; // Return true if a record already exists
                }
                catch (OleDbException ex)
                {
                    lblMessage.Text = "Error checking ResidentActivity for duplicates: " + ex.Message;
                    return false;
                }
            }
        }

        private void InsertResidentActivity(OleDbConnection dbCon, int waterConsumptionId, string username)
        {
            int activityNum = Convert.ToInt32(activityDD.SelectedValue); // Assume this is dynamic
            DateTime startDate = DateTime.Today;

            // We no longer block insertions based on duplicates, so this check is informational
            bool exists = ResidentActivityExists(dbCon, username, activityNum, startDate);
            if (exists)
            {
                lblMessage.Text = "An activity with this username, activity number, and date already exists, but we will add a new entry.";
            }

            // Proceed with inserting the record regardless of duplicates in other fields
            string sqlInsert = @"INSERT INTO [ResidentActivity] 
                         ([UserID], [Resident_Username], [Activity_Num], [ResidentActivity_StartDate], [WC_ID])
                         VALUES (@UserID, @Username, @ActivityNum, @StartDate, @WC_ID)";

            using (OleDbCommand insertCommand = new OleDbCommand(sqlInsert, dbCon))
            {
                int userID = GetUserID(username); // Method to get UserID
                int notificationID = GetNotificationID(); // Assuming you have this method

                insertCommand.Parameters.AddWithValue("@UserID", userID);
                insertCommand.Parameters.AddWithValue("@Username", username);
                insertCommand.Parameters.AddWithValue("@ActivityNum", activityNum);
                insertCommand.Parameters.AddWithValue("@StartDate", startDate);
                insertCommand.Parameters.AddWithValue("@WC_ID", waterConsumptionId); // Unique WC_ID

                try
                {
                    int result = insertCommand.ExecuteNonQuery();
                    if (result > 0)
                    {
                        lblMessage.Text = "Resident activity successfully logged!";
                    }
                    else
                    {
                        lblMessage.Text = "Error inserting resident activity.";
                    }
                }
                catch (OleDbException ex)
                {
                    lblMessage.Text = "OleDbException: " + ex.Message;
                }
            }
        }
        private int GetUserID(string username)
            {
                int userID = 0;
                string query = "SELECT [UserID] FROM [Resident] WHERE [Resident_Username] = @Username";

                string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (OleDbConnection dbCon = new OleDbConnection(cs))
                {
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
            private int GetNotificationID()
            {
                int notificationID = 0;
                string query = "SELECT MAX([NotificationID]) FROM [Notification]";

                string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (OleDbConnection dbCon = new OleDbConnection(cs))
                {
                    OleDbCommand cmd = new OleDbCommand(query, dbCon);

                    try
                    {
                        dbCon.Open();
                        notificationID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (OleDbException ex)
                    {
                        lblMessage.Text = "Error retrieving Notification ID: " + ex.Message;
                    }
                }

                return notificationID;
            }

        // Method to retrieve PropertyID for the current user
        private int GetUserPropertyID(string username)
        {
            int propertyID = 0;
            string query = "SELECT PropertyID FROM Resident WHERE Resident_Username = ?";

            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, dbCon))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    dbCon.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        propertyID = Convert.ToInt32(result);
                    }
                }
            }
            return propertyID;
        }

    }
} 
 
