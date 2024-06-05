<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ExamApp.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <h2>Giriş Yap</h2>
        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
        <asp:TextBox ID="txtKullaniciAdi" runat="server" Placeholder="Kullanıcı Adı"></asp:TextBox>
        <asp:TextBox ID="txtSifre" runat="server" TextMode="Password" Placeholder="Şifre"></asp:TextBox>
        <asp:Button ID="btnGiris" runat="server" Text="Giriş" OnClick="btnGiris_Click" />
    &nbsp;<br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </div>
</asp:Content>
