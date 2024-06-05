

    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Web.UI;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Xceed.Document.NET;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;


namespace ExamApp
{
    public partial class SinavOlusturma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDerslerDropdown();
            }

            
                        Random randomSik = new Random();
                        int dogruSik = randomSik.Next(1,5);
                        lblDersSec.Text = dogruSik.ToString();

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
                reader.Close();
            }
        }

        protected void ddlDersler_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateKonularDropdown();
        }

        private void PopulateKonularDropdown()
        {
            string dersID = ddlDersler.SelectedValue;
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT KonuID, KonuAdi FROM Konu WHERE DersID = @DersID", conn);
                cmd.Parameters.AddWithValue("@DersID", dersID);
                SqlDataReader reader = cmd.ExecuteReader();
                ddlKonular.DataSource = reader;
                ddlKonular.DataTextField = "KonuAdi";
                ddlKonular.DataValueField = "KonuID";
                ddlKonular.DataBind();
                reader.Close();
            }
        }

        protected void btnSinavOlustur_Click(object sender, EventArgs e)
        {
            int dersID = int.Parse(ddlDersler.SelectedValue);
            int konuID = int.Parse(ddlKonular.SelectedValue);
            int soruSayisi = int.Parse(txtSoruSayisi.Text);

            string sinavAdi = $"Sinav_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            string filePath = Server.MapPath("~/Dosyalar/Sinav.docx");

            using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                Paragraph titlePar = new Paragraph();
                Run titleRun = new Run(new Text($"Sınav: {sinavAdi}"));
                titlePar.AppendChild(titleRun);
                body.AppendChild(titlePar);

                Paragraph dersPar = new Paragraph();
                Run dersRun = new Run(new Text($"Ders: {ddlDersler.SelectedItem.Text}"));
                dersPar.AppendChild(dersRun);
                body.AppendChild(dersPar);

                Paragraph konuPar = new Paragraph();
                Run konuRun = new Run(new Text($"Konu: {ddlKonular.SelectedItem.Text}"));
                konuPar.AppendChild(konuRun);
                body.AppendChild(konuPar);

                body.AppendChild(new Paragraph(new Run()));

                Random rand = new Random();
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP (@SoruSayisi) SoruMetni FROM Soru WHERE KonuID = @KonuID ORDER BY NEWID()", conn);
                    cmd.Parameters.AddWithValue("@SoruSayisi", soruSayisi);
                    cmd.Parameters.AddWithValue("@KonuID", konuID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int soruNo = 1;
                    while (reader.Read())
                    {
                        Paragraph soruPar = new Paragraph();
                        Run soruRun = new Run(new Text($"{soruNo}. {reader["SoruMetni"].ToString()}"));
                        soruPar.AppendChild(soruRun);
                        body.AppendChild(soruPar);

                        for     (int i = 0; i < soruNo; i++)
                        {

                        }

                        soruNo++;
                    }
                    reader.Close();
                }
            }

            lblMesaj.Text = "Sınav başarıyla oluşturuldu.";
            lblMesaj.ForeColor = System.Drawing.Color.Green;
            btnIndir.Visible = true;
        }

        protected void btnIndir_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/Dosyalar/Sinav.docx");
            if (File.Exists(filePath))
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Sinav.docx");
                Response.TransmitFile(filePath);
                Response.End();
            }
            else
            {
                lblMesaj.Text = "Dosya bulunamadı.";
                lblMesaj.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}