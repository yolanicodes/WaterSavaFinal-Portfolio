using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WaterSavaFinal
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoginTime"] != null)
            {
                DateTime loginTime = (DateTime)Session["LoginTime"];
                if ((DateTime.Now - loginTime).TotalHours > 6)
                {
                    Session.Clear();
                    Response.Redirect("Login.aspx");
                }
            }
        }
        protected void logOut_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Clear the authentication cookie
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                HttpCookie authCookie = new HttpCookie(".ASPXAUTH");
                authCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authCookie);
            }

            // Clear the "Remember Me" cookie
            if (Request.Cookies["RememberMe"] != null)
            {
                HttpCookie rememberMeCookie = new HttpCookie("RememberMe");
                rememberMeCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(rememberMeCookie);
            }

            // Redirect to the login page
            Response.Redirect("~/Login.aspx");
        }

    }
}