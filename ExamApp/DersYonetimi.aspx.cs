using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class DersYonetimi : System.Web.UI.Page
    {

        protected void btnYeniDersEkle_Click(object sender, EventArgs e)
        {
            string dersAdi = txtYeniDersAdi.Text;

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Ders (DersAdi) VALUES (@DersAdi)", conn);
                cmd.Parameters.AddWithValue("@DersAdi", dersAdi);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect(Request.RawUrl);
        }
    }
}

