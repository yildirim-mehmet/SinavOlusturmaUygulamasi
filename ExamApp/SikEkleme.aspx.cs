using System;
using System.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamApp
{
    public partial class SikEkleme : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateSorularDropdown();
        }
    }

    private void PopulateSorularDropdown()
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT SoruID, SoruMetni FROM Soru", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            ddlSorular.DataSource = reader;
            ddlSorular.DataTextField = "SoruMetni";
            ddlSorular.DataValueField = "SoruID";
            ddlSorular.DataBind();
        }
    }

    protected void ddlSorular_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvSiklar.DataBind();
    }

    protected void btnSikEkle_Click(object sender, EventArgs e)
    {
        int soruID = int.Parse(ddlSorular.SelectedValue);
        string sikMetni = txtSikMetni.Text;
        bool dogruMu = chkDogruMu.Checked;

        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();

            if (dogruMu)
            {
                SqlCommand cmdUpdate = new SqlCommand("UPDATE Sik SET DogruMu = 0 WHERE SoruID = @SoruID", conn);
                cmdUpdate.Parameters.AddWithValue("@SoruID", soruID);
                cmdUpdate.ExecuteNonQuery();
            }

            SqlCommand cmdInsert = new SqlCommand("INSERT INTO Sik (SoruID, SikMetni, DogruMu) VALUES (@SoruID, @SikMetni, @DogruMu)", conn);
            cmdInsert.Parameters.AddWithValue("@SoruID", soruID);
            cmdInsert.Parameters.AddWithValue("@SikMetni", sikMetni);
            cmdInsert.Parameters.AddWithValue("@DogruMu", dogruMu);
            cmdInsert.ExecuteNonQuery();

            lblMesaj.Text = "Şık başarıyla eklendi.";
            gvSiklar.DataBind();
        }
    }
}

}


