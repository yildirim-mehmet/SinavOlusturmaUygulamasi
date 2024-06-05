using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ExamApp
{
    public partial class SinavOlusturma1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DersleriYukle();
            }
        }

        private void DersleriYukle()
        {
            string query = "SELECT DersID, DersAdi FROM Ders";
            ddlDersler.DataSource = GetData(query);
            ddlDersler.DataTextField = "DersAdi";
            ddlDersler.DataValueField = "DersID";
            ddlDersler.DataBind();
            ddlDersler.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Ders Seçin", "0"));
        }

        protected void ddlDersler_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "SELECT KonuID, KonuAdi FROM Konu WHERE DersID = @DersID";
            SqlParameter[] parameters = {
                new SqlParameter("@DersID", ddlDersler.SelectedValue)
            };
            ddlKonular.DataSource = GetData(query, parameters);
            ddlKonular.DataTextField = "KonuAdi";
            ddlKonular.DataValueField = "KonuID";
            ddlKonular.DataBind();
            ddlKonular.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Konu Seçin", "0"));
        }

        protected void btnSinavOlustur_Click(object sender, EventArgs e)
        {
            int dersID = int.Parse(ddlDersler.SelectedValue);
            int konuID = int.Parse(ddlKonular.SelectedValue);
            int soruSayisi = int.Parse(txtSoruSayisi.Text);
            int sikSayisi = int.Parse(txtSikSayisi.Text);
            bool zorSorular = chkZorSorular.Checked;

            if (dersID == 0 || konuID == 0 || soruSayisi == 0 || sikSayisi == 0)
            {
                lblMesaj.Text = "Lütfen tüm alanları doldurun.";
                lblMesaj.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string query = zorSorular
                ? @"SELECT TOP (@SoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND ZorlukSeviyesi = 1 ORDER BY NEWID()"
                : @"SELECT TOP (@SoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID ORDER BY NEWID()";

            SqlParameter[] parameters = {
                new SqlParameter("@SoruSayisi", soruSayisi),
                new SqlParameter("@KonuID", konuID)
            };

            DataTable dt = GetData(query, parameters);
            List<Soru> sorular = new List<Soru>();

            foreach (DataRow row in dt.Rows)
            {
                Soru soru = new Soru
                {
                    SoruID = int.Parse(row["SoruID"].ToString()),
                    SoruMetni = row["SoruMetni"].ToString(),
                    SIklar = SIkGetir(int.Parse(row["SoruID"].ToString()), sikSayisi)
                };
                sorular.Add(soru);
            }

            if (sorular.Count < soruSayisi)
            {
                lblMesaj.Text = "Veritabanında yeterli sayıda zor soru yok. Diğer sorulardan ekleniyor.";
                lblMesaj.ForeColor = System.Drawing.Color.Orange;

                int eksikSoruSayisi = soruSayisi - sorular.Count;

                string ekQuery = zorSorular
                    ? @"SELECT TOP (@EksikSoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND ZorlukSeviyesi = 1 AND SoruID NOT IN (" + string.Join(",", sorular.Select(s => s.SoruID)) + ") ORDER BY NEWID()"
                    : @"SELECT TOP (@EksikSoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND SoruID NOT IN (" + string.Join(",", sorular.Select(s => s.SoruID)) + ") ORDER BY NEWID()";

                SqlParameter[] ekParameters = {
                    new SqlParameter("@EksikSoruSayisi", eksikSoruSayisi),
                    new SqlParameter("@KonuID", konuID)
                };

                DataTable ekDt = GetData(ekQuery, ekParameters);
                foreach (DataRow row in ekDt.Rows)
                {
                    Soru soru = new Soru
                    {
                        SoruID = int.Parse(row["SoruID"].ToString()),
                        SoruMetni = row["SoruMetni"].ToString(),
                        SIklar = SIkGetir(int.Parse(row["SoruID"].ToString()), sikSayisi)
                    };
                    sorular.Add(soru);
                }
            }

            if (sorular.Count < soruSayisi)
            {
                lblMesaj.Text = "Veritabanında yeterli sayıda soru yok. Mevcut sorularla sınav oluşturuluyor.";
                lblMesaj.ForeColor = System.Drawing.Color.Red;
            }

            // Sınav Belgesi Oluşturma
            string sinavAdiA = $"Sinav_A_{dersID}_{konuID}_{DateTime.Now.Ticks}.docx";
            string sinavAdiB = $"Sinav_B_{dersID}_{konuID}_{DateTime.Now.Ticks}.docx";
            string dosyaYoluA = Server.MapPath("~/Dosyalar/" + sinavAdiA);
            string dosyaYoluB = Server.MapPath("~/Dosyalar/" + sinavAdiB);

            // A Grubu Sınavı Oluşturma
            OlusturSinavBelgesi(sorular, dosyaYoluA);

            // B Grubu Sınavı Oluşturma (Soru sıralaması rastgele değişecek)
            List<Soru> karisikSorular = sorular.OrderBy(x => Guid.NewGuid()).ToList();
            foreach (var soru in karisikSorular)
            {
                soru.SIklar = soru.SIklar.OrderBy(x => Guid.NewGuid()).ToList();
            }
            OlusturSinavBelgesi(karisikSorular, dosyaYoluB);

            lblMesaj.Text += " Sınav başarıyla oluşturuldu. İndir: ";
            lblMesaj.ForeColor = System.Drawing.Color.Green;

            HyperLink linkA = new HyperLink();
            linkA.Text = "A Grubu Sınavı indir";
            linkA.NavigateUrl = "~/Dosyalar/" + sinavAdiA;
            lblMesaj.Controls.Add(linkA);

            lblMesaj.Controls.Add(new Literal { Text = "<br />" });

            HyperLink linkB = new HyperLink();
            linkB.Text = "B Grubu Sınavı indir";
            linkB.NavigateUrl = "~/Dosyalar/" + sinavAdiB;
            lblMesaj.Controls.Add(linkB);
        }

        private void OlusturSinavBelgesi(List<Soru> sorular, string dosyaYolu)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(dosyaYolu, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();

                Paragraph heading = new Paragraph();
                Run headingRun = new Run();
                headingRun.Append(new Text("Sınav"));
                heading.Append(headingRun);
                body.Append(heading);

                int soruNumarasi = 1;
                foreach (var soru in sorular)
                {
                    Paragraph soruParagraf = new Paragraph(new Run(new Text($"{soruNumarasi}. {soru.SoruMetni}")));
                    body.Append(soruParagraf);

                    char sikHarf = 'A';
                    foreach (var sik in soru.SIklar)
                    {
                        Paragraph sikParagraf = new Paragraph(new Run(new Text($"{sikHarf}) {sik.SikMetni}")));
                        body.Append(sikParagraf);
                        sikHarf++;
                    }
                    soruNumarasi++;
                    body.Append(new Paragraph(new Run(new Text(" "))));
                }

                Paragraph cevapAnahtariHeading = new Paragraph(new Run(new Text("Cevap Anahtarı:")));
                body.Append(cevapAnahtariHeading);

                soruNumarasi = 1;
                foreach (var soru in sorular)
                {
                    char dogruSikHarf = 'A';
                    foreach (var sik in soru.SIklar)
                    {
                        if (sik.DogruMu)
                        {
                            Paragraph cevapParagraf = new Paragraph(new Run(new Text($"{soruNumarasi}. {dogruSikHarf}")));
                            body.Append(cevapParagraf);
                            break;
                        }
                        dogruSikHarf++;
                    }
                    soruNumarasi++;
                }

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }
        }

        private List<SIk> SIkGetir(int soruID, int sikSayisi)
        {
            string query = "SELECT TOP (@SikSayisi) SikID, SikMetni, DogruMu FROM Sik WHERE SoruID = @SoruID ORDER BY NEWID()";
            SqlParameter[] parameters = {
                new SqlParameter("@SikSayisi", sikSayisi),
                new SqlParameter("@SoruID", soruID)
            };

            DataTable dt = GetData(query, parameters);
            List<SIk> siklar = new List<SIk>();

            foreach (DataRow row in dt.Rows)
            {
                SIk sik = new SIk
                {
                    SikID = int.Parse(row["SikID"].ToString()),
                    SikMetni = row["SikMetni"].ToString(),
                    DogruMu = bool.Parse(row["DogruMu"].ToString())
                };
                siklar.Add(sik);
            }
            return siklar;
        }

        private DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }

    public class Soru
    {
        public int SoruID { get; set; }
        public string SoruMetni { get; set; }
        public List<SIk> SIklar { get; set; }
        public string DogruSik => SIklar.FirstOrDefault(s => s.DogruMu)?.SikMetni;
    }

    public class SIk
    {
        public int SikID { get; set; }
        public string SikMetni { get; set; }
        public bool DogruMu { get; set; }
    }
}




//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web.UI.WebControls;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;

//namespace ExamApp
//{
//    public partial class SinavOlusturma1 : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                DersleriYukle();
//            }
//        }

//        private void DersleriYukle()
//        {
//            string query = "SELECT DersID, DersAdi FROM Ders";
//            ddlDersler.DataSource = GetData(query);
//            ddlDersler.DataTextField = "DersAdi";
//            ddlDersler.DataValueField = "DersID";
//            ddlDersler.DataBind();
//            ddlDersler.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Ders Seçin", "0"));
//        }

//        protected void ddlDersler_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string query = "SELECT KonuID, KonuAdi FROM Konu WHERE DersID = @DersID";
//            SqlParameter[] parameters = {
//                new SqlParameter("@DersID", ddlDersler.SelectedValue)
//            };
//            ddlKonular.DataSource = GetData(query, parameters);
//            ddlKonular.DataTextField = "KonuAdi";
//            ddlKonular.DataValueField = "KonuID";
//            ddlKonular.DataBind();
//            ddlKonular.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Konu Seçin", "0"));
//        }

//        protected void btnSinavOlustur_Click(object sender, EventArgs e)
//        {
//            int dersID = int.Parse(ddlDersler.SelectedValue);
//            int konuID = int.Parse(ddlKonular.SelectedValue);
//            int soruSayisi = int.Parse(txtSoruSayisi.Text);
//            int sikSayisi = int.Parse(txtSikSayisi.Text);
//            bool zorSorular = chkZorSorular.Checked;

//            if (dersID == 0 || konuID == 0 || soruSayisi == 0 || sikSayisi == 0)
//            {
//                lblMesaj.Text = "Lütfen tüm alanları doldurun.";
//                lblMesaj.ForeColor = System.Drawing.Color.Red;
//                return;
//            }

//            string query = zorSorular
//                ? @"SELECT TOP (@SoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND ZorlukSeviyesi = 1 ORDER BY NEWID()"
//                : @"SELECT TOP (@SoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID ORDER BY NEWID()";

//            SqlParameter[] parameters = {
//                new SqlParameter("@SoruSayisi", soruSayisi),
//                new SqlParameter("@KonuID", konuID)
//            };

//            DataTable dt = GetData(query, parameters);
//            List<Soru> sorular = new List<Soru>();

//            foreach (DataRow row in dt.Rows)
//            {
//                Soru soru = new Soru
//                {
//                    SoruID = int.Parse(row["SoruID"].ToString()),
//                    SoruMetni = row["SoruMetni"].ToString(),
//                    SIklar = SIkGetir(int.Parse(row["SoruID"].ToString()), sikSayisi)
//                };
//                sorular.Add(soru);
//            }

//            if (sorular.Count < soruSayisi)
//            {
//                lblMesaj.Text = "Veritabanında yeterli sayıda zor soru yok. Diğer sorulardan ekleniyor.";
//                lblMesaj.ForeColor = System.Drawing.Color.Orange;

//                int eksikSoruSayisi = soruSayisi - sorular.Count;

//                string ekQuery = zorSorular
//                    ? @"SELECT TOP (@EksikSoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND ZorlukSeviyesi = 1 AND SoruID NOT IN (" + string.Join(",", sorular.Select(s => s.SoruID)) + ") ORDER BY NEWID()"
//                    : @"SELECT TOP (@EksikSoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND SoruID NOT IN (" + string.Join(",", sorular.Select(s => s.SoruID)) + ") ORDER BY NEWID()";

//                SqlParameter[] ekParameters = {
//                    new SqlParameter("@EksikSoruSayisi", eksikSoruSayisi),
//                    new SqlParameter("@KonuID", konuID)
//                };

//                DataTable ekDt = GetData(ekQuery, ekParameters);
//                foreach (DataRow row in ekDt.Rows)
//                {
//                    Soru soru = new Soru
//                    {
//                        SoruID = int.Parse(row["SoruID"].ToString()),
//                        SoruMetni = row["SoruMetni"].ToString(),
//                        SIklar = SIkGetir(int.Parse(row["SoruID"].ToString()), sikSayisi)
//                    };
//                    sorular.Add(soru);
//                }
//            }

//            if (sorular.Count < soruSayisi)
//            {
//                lblMesaj.Text = "Veritabanında yeterli sayıda soru yok. Mevcut sorularla sınav oluşturuluyor.";
//                lblMesaj.ForeColor = System.Drawing.Color.Red;
//            }

//            // Sınav Belgesi Oluşturma
//            string sinavAdiA = $"Sinav_A_{dersID}_{konuID}_{DateTime.Now.Ticks}.docx";
//            string sinavAdiB = $"Sinav_B_{dersID}_{konuID}_{DateTime.Now.Ticks}.docx";
//            string dosyaYoluA = Server.MapPath("~/Dosyalar/" + sinavAdiA);
//            string dosyaYoluB = Server.MapPath("~/Dosyalar/" + sinavAdiB);

//            // A Grubu Sınavı Oluşturma
//            OlusturSinavBelgesi(sorular, dosyaYoluA);

//            // B Grubu Sınavı Oluşturma (Soru sıralaması rastgele değişecek)
//            List<Soru> karisikSorular = sorular.OrderBy(x => Guid.NewGuid()).ToList();
//            foreach (var soru in karisikSorular)
//            {
//                soru.SIklar = soru.SIklar.OrderBy(x => Guid.NewGuid()).ToList();
//            }
//            OlusturSinavBelgesi(karisikSorular, dosyaYoluB);

//            lblMesaj.Text += " Sınav başarıyla oluşturuldu. İndir: ";
//            lblMesaj.ForeColor = System.Drawing.Color.Green;

//            HyperLink linkA = new HyperLink();
//            linkA.Text = "A Grubu Sınavı indir";
//            linkA.NavigateUrl = "~/Dosyalar/" + sinavAdiA;
//            lblMesaj.Controls.Add(linkA);

//            lblMesaj.Controls.Add(new Literal { Text = "<br />" });

//            HyperLink linkB = new HyperLink();
//            linkB.Text = "B Grubu Sınavı indir";
//            linkB.NavigateUrl = "~/Dosyalar/" + sinavAdiB;
//            lblMesaj.Controls.Add(linkB);
//        }

//        private void OlusturSinavBelgesi(List<Soru> sorular, string dosyaYolu)
//        {
//            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(dosyaYolu, WordprocessingDocumentType.Document))
//            {
//                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
//                mainPart.Document = new Document();
//                Body body = new Body();

//                Paragraph heading = new Paragraph();
//                Run headingRun = new Run();
//                headingRun.Append(new Text("Sınav"));
//                heading.Append(headingRun);
//                body.Append(heading);

//                int soruNumarasi = 1;
//                foreach (var soru in sorular)
//                {
//                    Paragraph soruParagraf = new Paragraph(new Run(new Text($"{soruNumarasi}. {soru.SoruMetni}")));
//                    body.Append(soruParagraf);

//                    char sikHarf = 'A';
//                    foreach (var sik in soru.SIklar)
//                    {
//                        Paragraph sikParagraf = new Paragraph(new Run(new Text($"{sikHarf}) {sik.SikMetni}")));
//                        body.Append(sikParagraf);
//                        sikHarf++;
//                    }
//                    soruNumarasi++;
//                    body.Append(new Paragraph(new Run(new Text(" "))));
//                }

//                Paragraph cevapAnahtariHeading = new Paragraph(new Run(new Text("Cevap Anahtarı:")));
//                body.Append(cevapAnahtariHeading);

//                soruNumarasi = 1;
//                foreach (var soru in sorular)
//                {
//                    Paragraph cevapParagraf = new Paragraph(new Run(new Text($"{soruNumarasi}. {soru.DogruSik}")));
//                    body.Append(cevapParagraf);
//                    soruNumarasi++;
//                }

//                mainPart.Document.Append(body);
//                mainPart.Document.Save();
//            }
//        }

//        private List<SIk> SIkGetir(int soruID, int sikSayisi)
//        {
//            string query = "SELECT TOP (@SikSayisi) SikID, SikMetni, DogruMu FROM Sik WHERE SoruID = @SoruID ORDER BY NEWID()";
//            SqlParameter[] parameters = {
//                new SqlParameter("@SikSayisi", sikSayisi),
//                new SqlParameter("@SoruID", soruID)
//            };

//            DataTable dt = GetData(query, parameters);
//            List<SIk> siklar = new List<SIk>();

//            foreach (DataRow row in dt.Rows)
//            {
//                SIk sik = new SIk
//                {
//                    SikID = int.Parse(row["SikID"].ToString()),
//                    SikMetni = row["SikMetni"].ToString(),
//                    DogruMu = bool.Parse(row["DogruMu"].ToString())
//                };
//                siklar.Add(sik);
//            }
//            return siklar;
//        }

//        private DataTable GetData(string query, SqlParameter[] parameters = null)
//        {
//            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
//            using (SqlConnection con = new SqlConnection(conString))
//            {
//                using (SqlCommand cmd = new SqlCommand(query, con))
//                {
//                    if (parameters != null)
//                    {
//                        cmd.Parameters.AddRange(parameters);
//                    }
//                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
//                    {
//                        DataTable dt = new DataTable();
//                        sda.Fill(dt);
//                        return dt;
//                    }
//                }
//            }
//        }
//    }

//    public class Soru
//    {
//        public int SoruID { get; set; }
//        public string SoruMetni { get; set; }
//        public List<SIk> SIklar { get; set; }
//        public string DogruSik => SIklar.FirstOrDefault(s => s.DogruMu)?.SikMetni;
//    }

//    public class SIk
//    {
//        public int SikID { get; set; }
//        public string SikMetni { get; set; }
//        public bool DogruMu { get; set; }
//    }
//}




////using System;
////using System.Collections.Generic;
////using System.Data;
////using System.Data.SqlClient;
////using System.Linq;
////using System.Web.UI.WebControls;
////using DocumentFormat.OpenXml;
////using DocumentFormat.OpenXml.Packaging;
////using DocumentFormat.OpenXml.Wordprocessing;

////namespace ExamApp
////{
////    public partial class SinavOlusturma1 : System.Web.UI.Page
////    {
////        protected void Page_Load(object sender, EventArgs e)
////        {
////            if (!IsPostBack)
////            {
////                DersleriYukle();
////            }
////        }

////        private void DersleriYukle()
////        {
////            string query = "SELECT DersID, DersAdi FROM Ders";
////            ddlDersler.DataSource = GetData(query);
////            ddlDersler.DataTextField = "DersAdi";
////            ddlDersler.DataValueField = "DersID";
////            ddlDersler.DataBind();
////            ddlDersler.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Ders Seçin", "0"));
////        }

////        protected void ddlDersler_SelectedIndexChanged(object sender, EventArgs e)
////        {
////            string query = "SELECT KonuID, KonuAdi FROM Konu WHERE DersID = @DersID";
////            SqlParameter[] parameters = {
////                new SqlParameter("@DersID", ddlDersler.SelectedValue)
////            };
////            ddlKonular.DataSource = GetData(query, parameters);
////            ddlKonular.DataTextField = "KonuAdi";
////            ddlKonular.DataValueField = "KonuID";
////            ddlKonular.DataBind();
////            ddlKonular.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Konu Seçin", "0"));
////        }

////        protected void btnSinavOlustur_Click(object sender, EventArgs e)
////        {
////            int dersID = int.Parse(ddlDersler.SelectedValue);
////            int konuID = int.Parse(ddlKonular.SelectedValue);
////            int soruSayisi = int.Parse(txtSoruSayisi.Text);
////            int sikSayisi = int.Parse(txtSikSayisi.Text);
////            bool zorSorular = chkZorSorular.Checked;

////            if (dersID == 0 || konuID == 0 || soruSayisi == 0 || sikSayisi == 0)
////            {
////                lblMesaj.Text = "Lütfen tüm alanları doldurun.";
////                lblMesaj.ForeColor = System.Drawing.Color.Red;
////                return;
////            }

////            string query = zorSorular
////                ? @"SELECT TOP (@SoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND ZorlukSeviyesi = 1 ORDER BY NEWID()"
////                : @"SELECT TOP (@SoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID ORDER BY NEWID()";

////            SqlParameter[] parameters = {
////                new SqlParameter("@SoruSayisi", soruSayisi),
////                new SqlParameter("@KonuID", konuID)
////            };

////            DataTable dt = GetData(query, parameters);
////            List<Soru> sorular = new List<Soru>();

////            foreach (DataRow row in dt.Rows)
////            {
////                Soru soru = new Soru
////                {
////                    SoruID = int.Parse(row["SoruID"].ToString()),
////                    SoruMetni = row["SoruMetni"].ToString(),
////                    SIklar = SIkGetir(int.Parse(row["SoruID"].ToString()), sikSayisi)
////                };
////                sorular.Add(soru);
////            }

////            if (sorular.Count < soruSayisi)
////            {
////                lblMesaj.Text = "Veritabanında yeterli sayıda zor soru yok. Diğer sorulardan ekleniyor.";
////                lblMesaj.ForeColor = System.Drawing.Color.Orange;

////                int eksikSoruSayisi = soruSayisi - sorular.Count;

////                string ekQuery = zorSorular
////                    ? @"SELECT TOP (@EksikSoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND ZorlukSeviyesi = 1 AND SoruID NOT IN (" + string.Join(",", sorular.Select(s => s.SoruID)) + ") ORDER BY NEWID()"
////                    : @"SELECT TOP (@EksikSoruSayisi) SoruID, SoruMetni FROM Soru WHERE KonuID = @KonuID AND SoruID NOT IN (" + string.Join(",", sorular.Select(s => s.SoruID)) + ") ORDER BY NEWID()";

////                SqlParameter[] ekParameters = {
////                    new SqlParameter("@EksikSoruSayisi", eksikSoruSayisi),
////                    new SqlParameter("@KonuID", konuID)
////                };

////                DataTable ekDt = GetData(ekQuery, ekParameters);
////                foreach (DataRow row in ekDt.Rows)
////                {
////                    Soru soru = new Soru
////                    {
////                        SoruID = int.Parse(row["SoruID"].ToString()),
////                        SoruMetni = row["SoruMetni"].ToString(),
////                        SIklar = SIkGetir(int.Parse(row["SoruID"].ToString()), sikSayisi)
////                    };

////                    // Dopru şıkkı rastgele seçme ve yerleştkirme
////                    Random rnd = new Random();
////                    int dogruSikIndex = rnd.Next(sorular.Count + 1); // Dogru şıkkı eklennek için rastgele bir indis seç
////                    soru.SIklar.Insert(dogruSikIndex, DogruSikGetir(soru.SoruID)); // Dogru dıkkı seçilen indise ekle

////                    sorular.Add(soru);
////                }
////            }

////            if (sorular.Count < soruSayisi)
////            {
////                lblMesaj.Text = "Veritabanında yeterli sayıda soru yok. Mevcut sorularla sınav oluşturuluyor.";
////                lblMesaj.ForeColor = System.Drawing.Color.Red;
////            }

////            // Sınav Velgesi Oluşturma
////            string sinavAdi = $"Sinav_{dersID}_{konuID}_{DateTime.Now.Ticks}.docx";
////            string dosyaYolu = Server.MapPath("~/Dosyalar/" + sinavAdi);

////            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(dosyaYolu, WordprocessingDocumentType.Document))
////            {
////                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
////                mainPart.Document = new Document();
////                Body body = new Body();

////                Paragraph heading = new Paragraph();
////                Run headingRun = new Run();
////                headingRun.Append(new Text("Sınav"));
////                heading.Append(headingRun);
////                body.Append(heading);

////                int soruNumarasi = 1;
////                foreach (var soru in sorular)
////                {
////                    Paragraph soruParagraf = new Paragraph(new Run(new Text($"{soruNumarasi}. {soru.SoruMetni}")));
////                    body.Append(soruParagraf);

////                    char sikHarf = 'A';
////                    foreach (var sik in soru.SIklar)
////                    {
////                        Paragraph sikParagraf = new Paragraph(new Run(new Text($"{sikHarf}) {sik.SikMetni}")));
////                        body.Append(sikParagraf);
////                        sikHarf++;
////                    }
////                    soruNumarasi++;
////                    body.Append(new Paragraph(new Run(new Text(" "))));
////                }

////                Paragraph cevapAnahtariHeading = new Paragraph(new Run(new Text("Cevap Anahtarı:")));
////                body.Append(cevapAnahtariHeading);

////                soruNumarasi = 1;
////                foreach (var soru in sorular)
////                {
////                    Paragraph cevapParagraf = new Paragraph(new Run(new Text($"{soruNumarasi}. {soru.DogruSik}")));
////                    body.Append(cevapParagraf);
////                    soruNumarasi++;
////                }

////                mainPart.Document.Append(body);
////                mainPart.Document.Save();
////            }

////            lblMesaj.Text += " Sınav başarıyla oluşturuldu. İndir: ";
////            lblMesaj.ForeColor = System.Drawing.Color.Green;
////            HyperLink link = new HyperLink();
////            link.Text = "Sınavı indir";
////            link.NavigateUrl = "~/Dosyalar/" + sinavAdi;
////            lblMesaj.Controls.Add(link);
////        }

////        private DataTable GetData(string query, SqlParameter[] parameters = null)
////        {
////            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
////            using (SqlConnection con = new SqlConnection(constr))
////            {
////                using (SqlCommand cmd = new SqlCommand(query, con))
////                {
////                    if (parameters != null)
////                    {
////                        cmd.Parameters.AddRange(parameters);
////                    }
////                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
////                    {
////                        DataTable dt = new DataTable();
////                        sda.Fill(dt);
////                        return dt;
////                    }
////                }
////            }
////        }
////        private List<SIk> SIkGetir(int soruID, int sikSayisi)
////        {
////            string query = @"SELECT TOP (@SikSayisi) SikID, SikMetni, DogruMu FROM Sik WHERE SoruID = @SoruID ORDER BY NEWID()";
////            SqlParameter[] parameters = {
////        new SqlParameter("@SikSayisi", sikSayisi),
////        new SqlParameter("@SoruID", soruID)
////    };

////            DataTable dt = GetData(query, parameters);
////            List<SIk> siklar = new List<SIk>();

////            foreach (DataRow row in dt.Rows)
////            {
////                SIk sik = new SIk
////                {
////                    SikID = int.Parse(row["SikID"].ToString()),
////                    SikMetni = row["SikMetni"].ToString(),
////                    DogruMu = bool.Parse(row["DogruMu"].ToString())
////                };
////                siklar.Add(sik);
////            }
////            return siklar;
////        }

////        private SIk DogruSikGetir(int soruID)
////        {
////            string query = @"SELECT SikID, SikMetni, DogruMu FROM Sik WHERE SoruID = @SoruID AND DogruMu = 1";
////            SqlParameter parameter = new SqlParameter("@SoruID", soruID);
////            DataTable dt = GetData(query, new SqlParameter[] { parameter });

////            if (dt.Rows.Count > 0)
////            {
////                DataRow row = dt.Rows[0];
////                SIk dogruSik = new SIk
////                {
////                    SikID = int.Parse(row["SikID"].ToString()),
////                    SikMetni = row["SikMetni"].ToString(),
////                    DogruMu = true // Dogru şık olduğu belirtiliyorrr
////                };
////                return dogruSik;
////            }
////            else
////            {
////                return null; // Dogru şık bulunamadı ise ysa null döndürülebilir veya hata işlenebilir
////            }
////        }


////        private class Soru
////        {
////            public int SoruID { get; set; }
////            public string SoruMetni { get; set; }
////            public List<SIk> SIklar { get; set; }
////            public string DogruSik
////            {
////                get
////                {
////                    for (int i = 0; i < SIklar.Count; i++)
////                    {
////                        if (SIklar[i].DogruMu)
////                        {
////                            return ((char)('A' + i)).ToString();
////                        }
////                    }
////                    return "";
////                }
////            }
////        }

////        private class SIk
////        {
////            public int SikID { get; set; }
////            public string SikMetni { get; set; }
////            public bool DogruMu { get; set; }
////        }
////    }
////}




