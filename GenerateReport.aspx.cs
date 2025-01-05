using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WaterSavaFinal
{
    public partial class GenerateReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserEmail"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    PopulateActivities();
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string userName = Session["UserEmail"].ToString();
            //String userName = "EsethuMnyombolo@gmail.com";
            DateTime StartDate = DateTime.Parse(startDate.Text);
            DateTime EndDate = DateTime.Parse(endDate.Text);
            string selectedActivity = activitiesDD.SelectedValue;

            DataTable dtConsumption = GetConsumptionData(userName, StartDate, EndDate, selectedActivity);

            if (dtConsumption != null && dtConsumption.Rows.Count > 0)
            {
                consumptionLV.DataSource = dtConsumption;
                consumptionLV.DataBind();
                BindData(dtConsumption);
                summaryPanel.Visible = true;
            }
            else
            {
                lblDebug.Text = "No data available for the selected criteria.";
                summaryPanel.Visible = false;
            }
        }
        private void BindData(DataTable dtConsumption)
        {
            consumptionChart.Series["ConsumptionSeries"].Points.Clear();
            List<string> colors = new List<string> { "#FF5733", "#33FF57", "#3357FF", "#F3FF33", "#FF33A8" }; // Define a list of colors
            int colorIndex = 0;

            foreach (DataRow row in dtConsumption.Rows)
            {
                var activityDescription = row["Activity_Description"].ToString();
                var consumptionValue = Convert.ToDouble(row["Consumption"]);

                // Add a point for each activity and get the index of the added point
                int pointIndex = consumptionChart.Series["ConsumptionSeries"].Points.AddXY(activityDescription, consumptionValue);

                // Access the point using the index and set its color and tooltip
                var point = consumptionChart.Series["ConsumptionSeries"].Points[pointIndex];
                point.Color = System.Drawing.ColorTranslator.FromHtml(colors[colorIndex % colors.Count]);
                point.ToolTip = $"Activity: {activityDescription}\nQuantity: {consumptionValue} Liters";

                colorIndex++; // Increment color index for the next activity
            }
            // Set titles for axes
            consumptionChart.ChartAreas[0].AxisX.Title = "Activity"; // X-axis title
            consumptionChart.ChartAreas[0].AxisY.Title = "Water Consumption (Liters)"; // Y-axis title
        }
        private DataTable GetConsumptionData(string username, DateTime startDate, DateTime endDate, string selectedActivity)
        {
            DataTable data = new DataTable();
            string query = @"
        SELECT A.[Activity_Description],WC.[WCDate], 
            SUM(IIF(WC.[WC_Metric] = 'ml', WC.[WCQuantityofWater] / 1000.0, WC.[WCQuantityofWater])) AS Consumption
        FROM ([ResidentActivity] RA
        INNER JOIN [WaterConsumption] WC ON RA.[WC_ID] = WC.[WC_ID])
        INNER JOIN [Activity] A ON RA.[Activity_Num] = A.[ActivityID]
        WHERE RA.[Resident_Username] = ? 
        AND WC.[WCDate] >= ? 
        AND WC.[WCDate] <= ?";

            // If an activity is selected, filter by that too
            if (!string.IsNullOrEmpty(selectedActivity))
            {
                query += " AND A.[ActivityID] = ?"; // Fix the ActivityID condition
            }

            query += " GROUP BY A.[Activity_Description], WC.[WCDate]"; // Add GROUP BY

            using (OleDbCommand cmd = new OleDbCommand(query))
            {
                cmd.Parameters.AddWithValue("?", username);
                cmd.Parameters.AddWithValue("?", startDate);
                cmd.Parameters.AddWithValue("?", endDate);

                // If selectedActivity is provided, add it to the parameters
                if (!string.IsNullOrEmpty(selectedActivity))
                {
                    cmd.Parameters.AddWithValue("?", selectedActivity);
                }

                data = GetData(cmd);
            }

            return data;
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

        private void PopulateActivities()
        {
            string username = Session["UserEmail"].ToString();
            
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();
                string query = @"SELECT a.[Activity_Description] AS ActivityDescription, a.[ActivityID]
                         FROM [ResidentActivity] ra
                         INNER JOIN [Activity] a ON ra.[Activity_Num] = a.[ActivityID]
                         WHERE ra.[Resident_Username] = ?"; 

                using (OleDbCommand cmd = new OleDbCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("?", username); 
                    try
                    {
                        OleDbDataReader reader = cmd.ExecuteReader();
                        HashSet<string> uniqueActivities = new HashSet<string>();
                        List<ListItem> activities = new List<ListItem>();

                        while (reader.Read())
                        {
                            string activityDescription = reader["ActivityDescription"].ToString();
                            string activityID = reader["ActivityID"].ToString();

                            
                            if (uniqueActivities.Add(activityDescription)) 
                            {
                                activities.Add(new ListItem(activityDescription, activityID));
                            }
                        }
                        activitiesDD.DataSource = activities;
                        activitiesDD.DataTextField = "Text"; 
                        activitiesDD.DataValueField = "Value"; 
                        activitiesDD.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblDebug.Text = ex.Message;
                    }
                    activitiesDD.Items.Insert(0, new ListItem("--Select Activity--", ""));
                }

                dbConnection.Close();
            }
        }

    }
}