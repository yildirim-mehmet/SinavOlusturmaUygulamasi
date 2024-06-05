using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class SinavYonetimi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDerslerDropdown();
            }
        }

        private void PopulateDerslerDropdown()
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DersID, DersAdi FROM Ders", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ddlDersler.DataSource = reader;
                ddlDersler.DataTextField = "DersAdi";
                ddlDersler.DataValueField = "DersID";
                ddlDersler.DataBind();
            }
        }

        protected void btnSinavEkle_Click(object sender, EventArgs e)
        {
            int dersID = int.Parse(ddlDersler.SelectedValue);
            string sinavAdi = txtSinavAdi.Text;

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Sinav (DersID, SinavAdi) VALUES (@DersID, @SinavAdi)", conn);
                cmd.Parameters.AddWithValue("@DersID", dersID);
                cmd.Parameters.AddWithValue("@SinavAdi", sinavAdi);
                cmd.ExecuteNonQuery();

                lblMesaj.Text = "Sınav başarıyla eklendi.";
                gvSinavlar.DataBind();
            }
        }
    }
}