# SinavOlusturmaUygulamasi		
 KTUN Proje Ödevi SinavOlusturmaUygulamasi		
 		
1	♦	Login olan kullanıcıların eklendiği silindiği,
2	♦	Sınav Derslerinin eklendiği silindiği,
3	♦	Ders Konularının eklendiği silindiği,
4	♦	(Derslerle eklenen konuların ilişkilendirildiği ve silindiği bir sayfa)
5	♦	Sınav sorularının oluşturulduğu (yanında zor soru olduğunu belirten checkbox) eklendiği silindiği,
6	♦	Oluşturulan sorulardan biri seçildikten sonra sınır sız sayıda şık oluşturulabildiği (şıkarın yanında doğru cevap seçeneği olan ve o sorunun tek doğru şıkkı bu olduğunu belirten checkbox ile tek doğru seçili bu şık)
7	♦	Ders Adı, Konusu ve soru sayısı (soruların şık sayısı) girilditikden sonra word üzerine rastgele seçtiği sorulardan tek doğru şıkkı seçmek kaydıyla rastgele seçtiği şıklardan soruları çıkartan ve A grubu B grubu şeklinde aynı soru ve şıklardan word halinde soru sayfası çıkartacak.
8	♦	hazırlanan sınava göre A ve B grubu için cevap anahtarları oluşturan Sistem...
9	♦	Sistemin veri tabanı MSSQL  (kullanıcı, ders, konu, sınav ve şıkların sql create scirptleri)
10	♦	Uygulama Asp.net WebForm WebApplication .net4,5 frame work ü üzerinde yapıldı.
		
11	♦	
		
12	♦	Örnek olarak Master page kullarak aşağıdaki sayfanın kodları eklendi;
		
13	♦	
		
14	♦	Master Page (AnaSayfa.master): Tüm sayfalarda ortak menü ve üst bilgileri barındıran ana sayfa şablonu.
15	♦	Giriş Sayfası (Login.aspx): Kullanıcıların sisteme giriş yapabileceği sayfa.
16	♦	Ana Sayfa (AnaSayfa.aspx): Sisteme giriş yapan kullanıcının ana sayfası.
17	♦	Kullanıcı Yönetimi (KullanıcıYonetimi.aspx): Yeni kullanıcı ekleme ve silme işlemlerinin yapılabileceği sayfa.
18	♦	//Sınav Yönetimi (SınavYonetimi.aspx): Yeni sınav ekleme ve silme işlemlerinin yapılabileceği sayfa.
19	♦	Ders Yönetimi (DersYonetimi.aspx): Yeni ders ekleme ve silme işlemlerinin yapılabileceği sayfa.
20	♦	Konu Yönetimi (KonuYonetimi.aspx): Yeni konu ekleme ve silme işlemlerinin yapılabileceği sayfa.
21	♦	Sınav Oluşturma (SinavOlusturma.aspx): Sınav için ders, konu ve soru seçerek rastgele sınav oluşturabileceğiniz sayfa.
22	♦	Soru Ekleme (SoruEkleme.aspx): Yeni soru ekleyebileceğiniz ve zorluk seviyesini belirleyebileceğiniz sayfa.
23	♦	Şık Ekleme (SikEkleme.aspx): Seçilen soru için şık ekleyebileceğiniz ve doğru cevabı işaretleyebileceğiniz sayfa.
24	♦	Raporlama (SinavOlusturma.aspx): Oluşturulan sınavlar için A ve B grubu soru ve cevap anahtarlarını görüntüleyebileceğiniz sayfa.
![image](https://github.com/yildirim-mehmet/SinavOlusturmaUygulamasi/assets/72050823/4b2ee3d9-b545-482e-8fe7-ead2f807b858)		
![image](https://github.com/yildirim-mehmet/SinavOlusturmaUygulamasi/assets/72050823/258c59d0-bed4-408e-8e6d-fddeaf6f6bf6)
