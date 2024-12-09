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

namespace WaterSavaFinal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadActivities();
            }
        }

        private void LoadChartInfo(DataTable dt)
        {

            Chart1.Series["Series1"].Points.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                Chart1.Series["Series1"].Points.AddXY(dr["Activity_Description"], dr["TotalAmount"]);
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
            string currentUser = Session["UserEmail"].ToString();
          
            DateTime today = DateTime.Now.Date;
            if (string.IsNullOrEmpty(currentUser))
            {
                lblMessage.Text = "UserEmail is null or empty.";
                return;
            }

            string query = @"SELECT [A].[Activity_Description] AS ActivityDescription, 
        SUM(IIF([WC].[WC_Metric] = 'ml' AND [WC].[WCQuantityofWater] IS NOT NULL, 
                [WC].[WCQuantityofWater] / 1000.0, 
                IIF([WC].[WC_Metric] = 'L', [WC].[WCQuantityofWater], 0))) AS TotalAmount FROM ([ResidentActivity] RA INNER JOIN [WaterConsumption] WC ON [RA].[WC_ID] = [WC].[WC_ID]) INNER JOIN [Activity] A ON [RA].[Activity_Num] = [A].[ActivityID] WHERE [RA].[Resident_Username] = ? AND [RA].[ResidentActivity_StartDate] = ? GROUP BY [A].[Activity_Description];

        ";
            using (OleDbCommand cmd = new OleDbCommand(query))
            {
                cmd.Parameters.AddWithValue("?", currentUser);
                cmd.Parameters.AddWithValue("?", today);

                try
                {
                    DataTable dt = GetData(cmd);
                    LoadChartInfo(dt);
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

                    dbCon.Close();
                }// Optionally, refresh the page to update the chart
                Response.Redirect(Request.RawUrl);
            }
        }

            private void InsertResidentActivity(OleDbConnection dbCon, int waterConsumptionId, string username)
            {
                // Define the SQL insert command for ResidentActivity
                string sqlInsert = "INSERT INTO [ResidentActivity] ([UserID], [Resident_Username], [Activity_Num], [ResidentActivity_StartDate], [ResidentActivity_EndDate], [ResidentActivity_Total],[NotificationID], [WC_ID]) VALUES (@UserID, @Username, @ActivityNum, @StartDate, @EndDate,@Total, @NotificationID,@WC_Id)";


            using (OleDbCommand insertCommand = new OleDbCommand(sqlInsert, dbCon))
                {
                    // Get the user ID based on the username
                    int userID = GetUserID(username);
                    int activityNum = Convert.ToInt32(activityDD.SelectedValue); // Get the activity number from dropdown
                    int notificationID = 1; // Get the notification ID (consider modifying this if necessary)
                int WaterTotal = 0;
                    DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today; // End of the day

                    // Add parameters for the insert command
                    insertCommand.Parameters.AddWithValue("@UserID", userID);
                    insertCommand.Parameters.AddWithValue("@Username", username);
                    insertCommand.Parameters.AddWithValue("@ActivityNum", activityNum); // This should be provided dynamically
                    insertCommand.Parameters.AddWithValue("@StartDate", startDate);
                    insertCommand.Parameters.AddWithValue("@EndDate", endDate);
                insertCommand.Parameters.AddWithValue("@Total", WaterTotal);
                insertCommand.Parameters.AddWithValue("@NotificationID", notificationID); // Assuming you have a method to get this ID
                    insertCommand.Parameters.AddWithValue("@WC_Id", waterConsumptionId); // Water consumption ID

                    // Execute the insert command
                    try
                    {
                       int  result = insertCommand.ExecuteNonQuery();
                
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
        }
    } 
 
