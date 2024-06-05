using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class AnaSayfa : System.Web.UI.MasterPage
    {

        protected void btnAra_Click(object sender, EventArgs e)
        {
            string aramaTerimi = txtArama.Text.Trim();
            Response.Redirect("AramaSonuclari.aspx?arama=" + Server.UrlEncode(aramaTerimi));
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Request.Url.AbsolutePath.EndsWith("login.aspx", StringComparison.OrdinalIgnoreCase) &&
                !Request.Url.AbsolutePath.EndsWith("logout.aspx", StringComparison.OrdinalIgnoreCase))
            {
                // Session kontrolü
                if (Session["KullaniciAdi"] == null)
                {
                    // Kullanıcı oturum açmamışsa login sayfasına yönlendirilir
                    Response.Redirect("login.aspx");
                }
                else
                {
                    // Kullanıcı adını göster
                    //lblUsern.Text = "Hoşgeldiniz, " + Session["KullaniciAdi"].ToString();
                }
            }

            //// Session kontrolü
            //if (Session["KullaniciAdi"] == null)
            //{
            //    // Kullanıcı oturum açmamışsa login sayfasına yönlendirilir
            //    Response.Redirect("login.aspx");
            //}
            //else
            //{
            //    Response.Redirect("default.aspx");
            //    // Kullanıcı adını göster
            //    //lblUsern.Text = "Hoşgeldiniz, " + Session["KullaniciAdi"].ToString();
            //}

            //var page = (Page)HttpContext.Current.CurrentHandler;
            //string url = page.AppRelativeVirtualPath;
            //// Session kontrolü
            //if (Session["KullaniciAdi"] == null)
            //{
            //    // Kullanıcı oturum açmamışsa login sayfasına yönlendirilir
            //    //Response.Redirect("login.aspx");
            //}
            //else
            //{
            //    // Kullanıcı adını 
            //   // lblUsern.Text = "Hoşgeldiniz, " + Session["KullaniciAdi"].ToString();
            //}

            //if (!IsPostBack)
            //{
            //    if (Session["KullaniciAdi"] == null || Session["KullaniciAdi"].ToString() == "")
            //    {
            //        Response.Redirect("login.aspx");
            //    }
            //    else
            //    {
            //        //lblUsern.Text = Session["KullaniciAdi"].ToString(); 
            //    }
            //}
        }
    }
}