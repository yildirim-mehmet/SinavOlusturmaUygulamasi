# SinavOlusturmaUygulamasi
 KTUN Proje Ödevi SinavOlusturmaUygulamasi

Login olan kullanıcıların eklendiği silindiği,
Sınav Derslerinin eklendiği silindiği,
Ders Konularının eklendiği silindiği,
(Derslerle eklenen konuların ilişkilendirildiği ve silindiği bir sayfa)
Sınav sorularının oluşturulduğu (yanında zor soru olduğunu belirten checkbox) eklendiği silindiği,
Oluşturulan sorulardan biri seçildikten sonra sınır sız sayıda şık oluşturulabildiği (şıkarın yanında doğru cevap seçeneği olan ve o sorunun tek doğru şıkkı bu olduğunu belirten checkbox ile tek doğru seçili bu şık)
Ders Adı, Konusu ve soru sayısı (soruların şık sayısı) girilditikden sonra word üzerine rastgele seçtiği sorulardan tek doğru şıkkı seçmek kaydıyla rastgele seçtiği şıklardan soruları çıkartan ve A grubu B grubu şeklinde aynı soru ve şıklardan word halinde soru sayfası çıkartacak.
hazırlanan sınava göre A ve B grubu için cevap anahtarları oluşturan Sistem...
Sistemin veri tabanı MSSQL  (kullanıcı, ders, konu, sınav ve şıkların sql create scirptleri)
Uygulama Asp.net WebForm WebApplication .net4,5 frame work ü üzerinde yapıldı.

Örnek olarak Master page kullarak aşağıdaki sayfanın kodları eklendi;

Master Page (AnaSayfa.master): Tüm sayfalarda ortak menü ve üst bilgileri barındıran ana sayfa şablonu.
Giriş Sayfası (Login.aspx): Kullanıcıların sisteme giriş yapabileceği sayfa.
Ana Sayfa (AnaSayfa.aspx): Sisteme giriş yapan kullanıcının ana sayfası.
Kullanıcı Yönetimi (KullanıcıYonetimi.aspx): Yeni kullanıcı ekleme ve silme işlemlerinin yapılabileceği sayfa.
//Sınav Yönetimi (SınavYonetimi.aspx): Yeni sınav ekleme ve silme işlemlerinin yapılabileceği sayfa.
Ders Yönetimi (DersYonetimi.aspx): Yeni ders ekleme ve silme işlemlerinin yapılabileceği sayfa.
Konu Yönetimi (KonuYonetimi.aspx): Yeni konu ekleme ve silme işlemlerinin yapılabileceği sayfa.
Sınav Oluşturma (SinavOlusturma.aspx): Sınav için ders, konu ve soru seçerek rastgele sınav oluşturabileceğiniz sayfa.
Soru Ekleme (SoruEkleme.aspx): Yeni soru ekleyebileceğiniz ve zorluk seviyesini belirleyebileceğiniz sayfa.
Şık Ekleme (SikEkleme.aspx): Seçilen soru için şık ekleyebileceğiniz ve doğru cevabı işaretleyebileceğiniz sayfa.
Raporlama (SinavOlusturma.aspx): Oluşturulan sınavlar için A ve B grubu soru ve cevap anahtarlarını görüntüleyebileceğiniz sayfa.
