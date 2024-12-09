using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Web;

namespace WaterSavaFinal
{
    public class WaterConsumptionService
        //For this to work , the user needs to load their home page 
    {
        private string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static double DailyLimit = 70.00;// DailyLimit

        public void CheckConsAndNotification( string email)
        {
            using (OleDbConnection dbCon = new OleDbConnection(cs))
            {
                dbCon.Open();

                string query = @"
                SELECT SUM(WC.WCQuantityofWater) AS DailyTotal
                FROM ResidentActivity RA
                INNER JOIN WaterConsumption WC ON RA.WC_ID = WC.WC_ID
                WHERE WC.WCDate = ?
                GROUP BY RA.UserID
                HAVING SUM(WC.WCQuantityofWater) >= ?";

                using (OleDbCommand command = new OleDbCommand(query, dbCon))
                {
                    command.Parameters.AddWithValue("@Date", DateTime.Today);
                    command.Parameters.AddWithValue("@Threshold", DailyLimit * 0.7);

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double dailyTotal = reader.GetDouble(0);



                            SendEmail(email, "Water Consumption Alert",
                                $"You have consumed {dailyTotal}ml, which is over 70% of the daily limit ({DailyLimit}ml). Please monitor your intake." +
                                $"" +
                                $"Your WaterSava");


                        }
                    }

                }


            }
        }

       

        private void SendEmail(string toAddress, string subject, string body)
        {
            using (SmtpClient client = new SmtpClient("smtp.office365.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("kwawatersava@outlook.com", "dgtxzmtjeyxyylsg");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("kwawatersava@outlook.com");
                mailMessage.To.Add(toAddress);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                client.Send(mailMessage);
            }
        }

    }
}