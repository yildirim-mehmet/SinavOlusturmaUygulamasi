using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Kullanici WHERE KullaniciAdi=@KullaniciAdi AND Sifre=@Sifre", conn);
                cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@Sifre", sifre);

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    // Kullanıcı adını Session'a kaydet
                    Session["KullaniciAdi"] = kullaniciAdi;

                    // Kimlik doğrulama çerezi oluştur
                    FormsAuthentication.SetAuthCookie(kullaniciAdi, false);

                    // Redirect işlemi
                    Response.Redirect("default.aspx");
                }
                else
                {
                    lblMessage.Text = "Geçersiz kullanıcı adı veya şifre.";
                }
            }
        }


        //protected void btnGiris_Click(object sender, EventArgs e)
        //{



        //    //string kullaniciAdi = txtKullaniciAdi.Text;
        //    //string sifre = txtSifre.Text;

        //    //if (Session["KullaniciAdi"] != null)
        //    //{
        //    //    string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        //    //    using (SqlConnection conn = new SqlConnection(connStr))
        //    //    {
        //    //        conn.Open();
        //    //        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Kullanici WHERE KullaniciAdi=@KullaniciAdi AND Sifre=@Sifre", conn);
        //    //        cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
        //    //        cmd.Parameters.AddWithValue("@Sifre", sifre);

        //    //        int count = (int)cmd.ExecuteScalar();
        //    //        if (count > 0)
        //    //        {
        //    //            Session["KullaniciAdi"] = kullaniciAdi;
        //    //            FormsAuthentication.RedirectFromLoginPage(kullaniciAdi, false);
        //    //        }
        //    //        else
        //    //        {
        //    //            lblMessage.Text = "Geçersiz kullanıcı adı veya şifre.";
        //    //        }
        //    //    }
        //    //}


        //}
    }
}