using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace WaterSavaFinal
{
    public partial class HouseTrack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = Session["UserEmail"].ToString(); 
                int userID = GetUserID(username); 

                if (userID != -1) 
                {
                    if (userID != -1) // Ensure UserID is valid
                    {
                        List<int> propertyIDs = GetAdminPropertyIDs(userID); 
                        PopulateHouses(propertyIDs); // Populate house dropdown using PropertyIDs
                        PopulateResidents(propertyIDs); // Populate residents dropdown using PropertyIDs
                    }

                }
                else
                {
                    lblDebug.Text = "User not found."; // Display an error if UserID is not found
                }

                PopulateActivities();
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string startDateText = startDate.Text;
            string endDateText = endDate.Text;
            string selectedActivity = activitiesDD.SelectedValue;
            string selectedResident = residentsDD.SelectedValue;

            DateTime parsedStartDate;
            DateTime parsedEndDate;

            // Validate the start and end dates
            bool isStartDateValid = DateTime.TryParse(startDateText, out parsedStartDate);
            bool isEndDateValid = DateTime.TryParse(endDateText, out parsedEndDate);

            int? selectedPropertyID = null;

            // Attempt to parse the selected house (property) ID from the dropdown
            if (int.TryParse(housesDD.SelectedValue, out int propertyID))
            {
                selectedPropertyID = propertyID;
            }

            if (isStartDateValid && isEndDateValid)
            {
                if (parsedStartDate > parsedEndDate)
                {
                    lblDebug.Text = "Start date must be before end date.";
                    return;
                }

                // Assuming the username is valid
                string username = Session["UserEmail"].ToString(); // Replace this with actual session retrieval if needed
                int userID = GetUserID(username);
                List<int> propertyIDs = GetAdminPropertyIDs(userID);

                if (propertyIDs.Count == 0)
                {
                    lblDebug.Text = "No properties found for the admin.";
                    return;
                }

                // Call GetConsumptionData with the correct types
                DataTable consumptionData = GetConsumptionData(username, parsedStartDate, parsedEndDate, selectedActivity, userID, selectedPropertyID, selectedResident);

                if (consumptionData.Rows.Count > 0)
                {
                    BindData(consumptionData); // Call BindData to display the chart
                    summaryPanel.Visible = true; // Ensure the panel containing the chart is visible
                    lblDebug.Text = "Data loaded successfully!";
                }
                else
                {
                    lblDebug.Text = "No data found for the selected criteria.";
                }
            }
            else
            {
                lblDebug.Text = "Please enter valid start and end dates.";
            }
        }

        private DataTable GetConsumptionData(string username, DateTime startDate, DateTime endDate, string selectedActivity, int userID, int? selectedPropertyID, string selectedResident)
        {
            DataTable data = new DataTable();

            string query = @"
        SELECT [A].[Activity_Description], 
       SUM(IIF([WC].[WC_Metric] = 'ml', [WC].[WCQuantityofWater] / 1000.0, [WC].[WCQuantityofWater])) AS TotalAmount 
FROM ((([ResidentActivity] RA 
        INNER JOIN [WaterConsumption] WC ON [RA].[WC_ID] = [WC].[WC_ID]) 
       INNER JOIN [Activity] A ON [RA].[Activity_Num] = [A].[ActivityID])
       INNER JOIN [Resident] R ON [RA].[Resident_Username] = [R].[Resident_Username])
       INNER JOIN [AdminProperty] AP ON [R].[PropertyID] = [AP].[PropertyID]
WHERE AP.[UserID] = ? 
          AND RA.[ResidentActivity_StartDate] BETWEEN ? AND ?";

            // Add the activity filter if selected
            if (!string.IsNullOrEmpty(selectedActivity))
            {
                query += " AND A.[ActivityID] = ?";
            }

            // Add the property filter if selected
            if (selectedPropertyID.HasValue)
            {
                query += " AND AP.[PropertyID] = ?";
            }

            // Add the resident filter if selected
            if (!string.IsNullOrEmpty(selectedResident))
            {
                query += " AND R.[Resident_Username] = ?";
            }

            query += " GROUP BY A.[Activity_Description]";

            using (OleDbCommand cmd = new OleDbCommand(query))
            {
                // Bind parameters for user and date range
                cmd.Parameters.AddWithValue("?", userID);
                cmd.Parameters.AddWithValue("?", startDate);
                cmd.Parameters.AddWithValue("?", endDate);

                // Bind the selected activity if needed
                if (!string.IsNullOrEmpty(selectedActivity))
                {
                    cmd.Parameters.AddWithValue("?", selectedActivity);
                }

                // Bind the selected property if needed
                if (selectedPropertyID.HasValue)
                {
                    cmd.Parameters.AddWithValue("?", selectedPropertyID.Value);
                }

                // Bind the selected resident if needed
                if (!string.IsNullOrEmpty(selectedResident))
                {
                    cmd.Parameters.AddWithValue("?", selectedResident);
                }

                data = GetData(cmd); // Fetch data using this command
            }

            return data;
        }

        private void BindData(DataTable dtConsumption)
        {
            // Clear any existing points in the series
            consumptionChart.Series["ConsumptionSeries"].Points.Clear();

            // Define a list of colors for the points
            List<string> colors = new List<string> { "#FF5733", "#33FF57", "#3357FF", "#F3FF33", "#FF33A8" };
            int colorIndex = 0; // To cycle through colors

            // Iterate through each row in the DataTable
            foreach (DataRow row in dtConsumption.Rows)
            {
                // Get the activity description and consumption value from the DataRow
                var activityDescription = row["Activity_Description"].ToString();
                var consumptionValue = Convert.ToDouble(row["TotalAmount"]);

                // Add a new point to the series and store its index
                int pointIndex = consumptionChart.Series["ConsumptionSeries"].Points.AddXY(activityDescription, consumptionValue);

                // Access the added point using its index to set custom properties
                var point = consumptionChart.Series["ConsumptionSeries"].Points[pointIndex];

                // Set the color of the point
                point.Color = System.Drawing.ColorTranslator.FromHtml(colors[colorIndex % colors.Count]);

                // Set tooltip for better clarity when hovered over
                point.ToolTip = $"Activity: {activityDescription}\nQuantity: {consumptionValue} Liters";

                // Move to the next color in the list
                colorIndex++;
            }

            // Set titles for the axes for better chart readability
            consumptionChart.ChartAreas[0].AxisX.Title = "Activity";
            consumptionChart.ChartAreas[0].AxisY.Title = "Water Consumption (Liters)";
        }




        private DataTable GetData(OleDbCommand cmd)
        {
            DataTable data = new DataTable();
            string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                dbCon.Open();
                cmd.Connection = dbCon;

                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    da.Fill(data);
                }
            }
            return data;
        }
        private void PopulateHouses(List<int> propertyIDs)
        {
            if (propertyIDs == null || propertyIDs.Count == 0)
            {
                lblDebug.Text = "No properties found for the user.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();
                string query = @"
            SELECT P.[Property_ID], P.[Property_HouseNum], P.[Property_StreetName], P.[Property_Suburb], 
                   P.[Property_City], P.[Property_Province], P.[Property_PostalCode]
            FROM [Property] P
            WHERE P.[Property_ID] IN (" + string.Join(",", propertyIDs) + ")"; // Filter using PropertyIDs

                using (OleDbCommand cmd = new OleDbCommand(query, dbConnection))
                {
                    try
                    {
                        OleDbDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string houseDetails = $"{reader["Property_HouseNum"]}, {reader["Property_StreetName"]}, {reader["Property_Suburb"]}, " +
                                                  $"{reader["Property_City"]}, {reader["Property_Province"]}, {reader["Property_PostalCode"]}";

                            ListItem item = new ListItem(houseDetails, reader["Property_ID"].ToString());
                            housesDD.Items.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblDebug.Text = "Error: " + ex.Message;
                    }

                    housesDD.Items.Insert(0, new ListItem("--Select House--", ""));
                }
            }
        }
        private void PopulateResidents(List<int> propertyIDs)
        {
            if (propertyIDs == null || propertyIDs.Count == 0)
            {
                lblDebug.Text = "No properties found for the user.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();

                // Create a query to retrieve residents linked to the admin's properties
                string query = @"SELECT DISTINCT R.[Resident_Username]
                         FROM [Resident] R
                         INNER JOIN [AdminProperty] AP ON R.[PropertyID] = AP.[PropertyID]
                         WHERE AP.[UserID] = ?";

                using (OleDbCommand cmd = new OleDbCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("?", GetUserID("nasoyako@icloud.com")); // Replace with actual session value

                    try
                    {
                        OleDbDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ListItem item = new ListItem(reader["Resident_Username"].ToString(), reader["Resident_Username"].ToString());
                            residentsDD.Items.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblDebug.Text = "Error: " + ex.Message;
                    }

                    residentsDD.Items.Insert(0, new ListItem("--Select Resident--", ""));
                }
            }
        }


        private void PopulateActivities()
        {
            string username = "nasoyako@icloud.com"; // Replace with actual session value
            int userID = GetUserID(username); // Get UserID using the username

            if (userID == -1)
            {
                lblDebug.Text = "User not found.";
                return;
            }

            // Get the property IDs associated with the user
            List<int> propertyIDs = GetAdminPropertyIDs(userID);

            if (propertyIDs.Count == 0)
            {
                lblDebug.Text = "No properties found for this user.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();

                // Create a query that retrieves activities linked to the user's properties
                string query = @"SELECT DISTINCT a.[Activity_Description] AS ActivityDescription, a.[ActivityID]
                         FROM [Activity] a
                         INNER JOIN [Property] p ON a.[Property_ID] = p.[Property_ID]
                         WHERE p.[Property_ID] IN (" + string.Join(",", propertyIDs) + ")";

                using (OleDbCommand cmd = new OleDbCommand(query, dbConnection))
                {
                    try
                    {
                        OleDbDataReader reader = cmd.ExecuteReader();
                        activitiesDD.DataSource = reader;
                        activitiesDD.DataTextField = "ActivityDescription";
                        activitiesDD.DataValueField = "ActivityID";
                        activitiesDD.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblDebug.Text = "Error loading activities: " + ex.Message;
                    }

                    activitiesDD.Items.Insert(0, new ListItem("--Select Activity--", ""));
                }
            }
        }

        private List<int> GetAdminPropertyIDs(int userID)
        {
            List<int> propertyIDs = new List<int>();
            string query = @"SELECT [PropertyID]
                     FROM [AdminProperty] 
                     WHERE [UserID] = ?";

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, dbConnection))
                {
                    cmd.Parameters.AddWithValue("?", userID); // Use UserID instead of username
                    try
                    {
                        OleDbDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            propertyIDs.Add(reader.GetInt32(0)); // Add PropertyID to list
                        }
                    }
                    catch (Exception ex)
                    {
                        lblDebug.Text = "Error retrieving PropertyIDs: " + ex.Message;
                    }
                }
            }
            return propertyIDs; // Return list of PropertyIDs
        }
        private int GetUserID(string username)
        {
            int userId = -1;
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT User_ID FROM [User] WHERE User_Email = ?";

            using (OleDbConnection conn = new OleDbConnection(CS))
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("?", username); // Bind the parameter correctly

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        userId = id;
                    }
                }
                catch (Exception ex)
                {
                    lblDebug.Text = "Error retrieving user ID: " + ex.Message;
                }
            }

            return userId;
        }

    }
}
