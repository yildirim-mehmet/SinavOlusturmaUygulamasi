using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class KonuYonetimi : System.Web.UI.Page
    {
        protected void btnYeniKonuEkle_Click(object sender, EventArgs e)
        {
            string konuAdi = txtYeniKonuAdi.Text;
            int dersID = int.Parse(ddlDers.SelectedValue);

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Konu (KonuAdi, DersID) VALUES (@KonuAdi, @DersID)", conn);
                cmd.Parameters.AddWithValue("@KonuAdi", konuAdi);
                cmd.Parameters.AddWithValue("@DersID", dersID);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect(Request.RawUrl);
        }
    }
}