using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class KullaniciYonetimi : System.Web.UI.Page
    {
        protected void btnYeniKullaniciEkle_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtYeniKullaniciAdi.Text;
            string sifre = txtYeniSifre.Text;

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Kullanici (KullaniciAdi, Sifre) VALUES (@KullaniciAdi, @Sifre)", conn);
                cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@Sifre", sifre);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect(Request.RawUrl);
        }

    }
}