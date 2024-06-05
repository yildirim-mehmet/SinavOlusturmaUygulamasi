using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            // Kullanıcı oturumunu sonlandır
            Session.Abandon();
            FormsAuthentication.SignOut();

            // Çerezleri temizle
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(authCookie);

            // Giriş sayfasına yönlendir
            Response.Redirect("login.aspx");
        }
    }
}