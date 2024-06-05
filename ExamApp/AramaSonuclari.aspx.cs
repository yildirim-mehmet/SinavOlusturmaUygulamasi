using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class AramaSonuclari : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string aramaTerimi = Request.QueryString["arama"];
                if (!string.IsNullOrEmpty(aramaTerimi))
                {
                    Ara(aramaTerimi);
                }
            }
        }

        private void Ara(string aramaTerimi)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Soru WHERE SoruMetni LIKE @AramaTerimi", conn);
                cmd.Parameters.AddWithValue("@AramaTerimi", "%" + aramaTerimi + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvSonuclar.DataSource = dt;
                gvSonuclar.DataBind();
            }
        }
    }
}






