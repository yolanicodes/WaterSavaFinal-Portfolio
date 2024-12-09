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
    public partial class indexAdmin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                //LoadActivities();
            }
        }

      
//        private void LoadActivities()
//        {
//            string currentAdmin = Session["UserEmail"]?.ToString();
//            if (string.IsNullOrEmpty(currentAdmin))
//            {
//                lblMessage.Text = "Admin email is null or empty.";
//                Response.Redirect("Login.aspx");
//                return;
//            }

//            DateTime today = DateTime.Now.Date;

//            string query = @"
//    SELECT [A].[Activity_Description],
//           SUM([WC].[WCQuantityofWater] / 1000.0) AS TotalAmount
//    FROM ([AdminProperty] AS [AP]
//    INNER JOIN [Property] AS [P] ON [AP].[PropertyID] = [P].[Property_ID]
//    INNER JOIN [Resident] AS [R] ON [R].[PropertyID] = [P].[Property_ID]
//    INNER JOIN [ResidentActivity] AS [RA] ON [RA].[UserID] = [R].[UserID]
//    INNER JOIN [WaterConsumption] AS [WC] ON [RA].[WC_ID] = [WC].[WC_ID]
//    INNER JOIN [Activity] AS [A] ON [RA].[Activity_Num] = [A].[ActivityID])
//    WHERE [AP].[UserID] = (SELECT [User_ID] FROM [User] WHERE [User_Email] = ?)
//      AND [RA].[ResidentActivity_StartDate] = ?
//    GROUP BY [A].[Activity_Description];
//";




//            using (OleDbCommand cmd = new OleDbCommand(query))
//            {
//                cmd.Parameters.AddWithValue("?", currentAdmin);
//                cmd.Parameters.AddWithValue("?", today);

//                try
//                {
//                    DataTable dt = GetData(cmd);
//                    if (dt.Rows.Count > 0)
//                    {
//                        LoadChartInfo(dt);
//                    }
//                    else
//                    {
//                        lblMessage.Text = "No water consumption data found for today.";
//                        Chart1.Visible = false;  // Hide the chart if there's no data
//                    }
//                }
//                catch (OleDbException ex)
//                {
//                    lblMessage.Text = "OleDbException: " + ex.Message;
//                }
//            }
//        }



//        private void LoadChartInfo(DataTable dt)
//        {
//            Chart1.Series["Series1"].Points.Clear();

//            if (dt.Rows.Count > 0 && dt.Columns.Contains("Activity_Description") && dt.Columns.Contains("TotalAmount"))
//            {
//                foreach (DataRow dr in dt.Rows)
//                {
//                    string activityDescription = dr["Activity_Description"].ToString();
//                    double totalAmount = Convert.ToDouble(dr["TotalAmount"]);

//                    // Manually create a new DataPoint
//                    DataPoint point = new DataPoint();
//                    point.SetValueXY(activityDescription, totalAmount);

//                    // Set the tooltip for the data point
//                    point.ToolTip = $"Activity: {activityDescription}\nTotal Consumption: {totalAmount} L";

//                    // Add the DataPoint to the series
//                    Chart1.Series["Series1"].Points.Add(point);
//                }
//            }
//            else
//            {
//                lblMessage.Text = "No data available or incorrect column names.";
//            }
//        }
//        private DataTable GetData(OleDbCommand cmd)
//        {
//            DataTable data = new DataTable();
//            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
//            using (OleDbConnection dbCon = new OleDbConnection(cs))
//            {
//                // Open the connection
//                dbCon.Open();
//                cmd.Connection = dbCon;

//                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
//                {
//                    da.Fill(data);
//                }
//            }
//            return data;
//        }
    }
}
