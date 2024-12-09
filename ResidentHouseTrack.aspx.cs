using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WaterSavaFinal
{
    public partial class ResidentHouseTracking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the page is not reloading on postback
            if (!IsPostBack)
            {
                // Fetch and display water consumption data for all residents of a specific house
                LoadWaterConsumptionData();
            }
        }
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
        private void LoadWaterConsumptionData()
        {
            string currentUser = Session["UserEmail"]?.ToString();
            DateTime today = DateTime.Now.Date; // Get today's date

            if (string.IsNullOrEmpty(currentUser))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int propertyID = GetUserPropertyID(currentUser); // Get property ID for the current user

            // SQL query to fetch water consumption by residents for today only
            string consumptionQuery = @"
                SELECT 
            RA.Resident_Username, 
            SUM(IIF(WC.WC_Metric = 'ml', WC.WCQuantityofWater / 1000.0, WC.WCQuantityofWater)) AS TotalConsumption
        FROM 
            (ResidentActivity RA 
            INNER JOIN WaterConsumption WC ON RA.WC_ID = WC.WC_ID)
            INNER JOIN Activity A ON RA.Activity_Num = A.ActivityID
        WHERE 
            RA.ResidentActivity_StartDate = ? 
            AND A.Property_ID = ?
        GROUP BY 
            RA.Resident_Username
        ORDER BY 
            SUM(IIF(WC.WC_Metric = 'ml', WC.WCQuantityofWater / 1000.0, WC.WCQuantityofWater)) ASC;
            ";

            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                OleDbCommand consumptionCmd = new OleDbCommand(consumptionQuery, dbCon);

                // Use today's date directly as parameter
                consumptionCmd.Parameters.AddWithValue("?", today);  // Use today's date for start date condition
                consumptionCmd.Parameters.AddWithValue("?", propertyID);  // Use the property ID for the current user

                try
                {
                    dbCon.Open();
                    OleDbDataReader reader = consumptionCmd.ExecuteReader();

                    if (reader.HasRows)  // Check if there are rows in the result set
                    {
                        // Bind the data to GridView
                        consumptionGridView.DataSource = reader;
                        consumptionGridView.DataBind();
                    }
                    else
                    {
                        // No data found for today, display a message
                        lblDebug.Text = "No water consumption data available for today.";
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors (e.g., database connection failure, query error)
                    lblDebug.Text = "Error: " + ex.Message;
                }
            }
        }
    }

    
}