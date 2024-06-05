using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



using System;
using System.Data.SqlClient;


namespace ExamApp
{

    public partial class SoruEkleme : System.Web.UI.Page
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

    protected void ddlDersler_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateKonularDropdown();
    }

    private void PopulateKonularDropdown()
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT KonuID, KonuAdi FROM Konu WHERE DersID = @DersID", conn);
            cmd.Parameters.AddWithValue("@DersID", ddlDersler.SelectedValue);
            SqlDataReader reader = cmd.ExecuteReader();
            ddlKonular.DataSource = reader;
            ddlKonular.DataTextField = "KonuAdi";
            ddlKonular.DataValueField = "KonuID";
            ddlKonular.DataBind();
        }
    }

    protected void btnSoruEkle_Click(object sender, EventArgs e)
    {
        int konuID = int.Parse(ddlKonular.SelectedValue);
        string soruMetni = txtSoruMetni.Text;
        bool zorlukSeviyesi = chkZorSoru.Checked;

        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Soru (KonuID, SoruMetni, ZorlukSeviyesi) VALUES (@KonuID, @SoruMetni, @ZorlukSeviyesi)", conn);
            cmd.Parameters.AddWithValue("@KonuID", konuID);
            cmd.Parameters.AddWithValue("@SoruMetni", soruMetni);
            cmd.Parameters.AddWithValue("@ZorlukSeviyesi", zorlukSeviyesi);
            cmd.ExecuteNonQuery();

            lblMesaj.Text = "Soru başarıyla eklendi.";
            gvSorular.DataBind();
        }
    }
}

}