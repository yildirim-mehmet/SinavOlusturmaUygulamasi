<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ExamApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KTUN SINAV Olusturma Uygulaması</title>
    <meta charset="UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
		<link rel="stylesheet" href="css/bootstrap.min.css" />
		<link rel="stylesheet" href="css/bootstrap-responsive.min.css" />
        <link rel="stylesheet" href="css/maruti-login.css" />
</head>
<body>
    <form id="form1" class="form-vertical"  runat="server">


        <div id="loginbox">            
 
		 <div class="control-group normal_text"> <h3><img src="img/ktun.png" alt="Logo" /></h3></div>
        <div class="control-group">
            <div class="controls">
                <div class="main_input_box">
                    <span class="add-on"><i class="icon-user"></i></span>
                    <asp:TextBox ID="txtKullaniciAdi" runat="server" Placeholder="Kullanıcı Adı"></asp:TextBox>
                   <%-- <input type="text" placeholder="Username" />--%>
                </div>
            </div>
        </div>
        <div class="control-group">
            <div class="controls">
                <div class="main_input_box">
                    <span class="add-on"><i class="icon-lock"></i></span><asp:TextBox ID="txtSifre" runat="server" TextMode="Password" Placeholder="Şifre"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-actions">
            <%--<span class="pull-left"><a href="#" class="flip-link btn btn-inverse" id="to-recover">Lost password?</a></span>--%>
            <span class="pull-right"> <asp:Button ID="btnGiris" runat="server"  CssClass="flip-link btn btn-inverse" Text="Giriş" OnClick="btnGiris_Click" /></span></div>

		<p class="normal_text"><asp:Label ID="lblMessage" runat="server" CssClass="controls label-info" Text="" ForeColor="Red"></asp:Label></p>
		

       

</div>



    </form>
</body>
</html>
