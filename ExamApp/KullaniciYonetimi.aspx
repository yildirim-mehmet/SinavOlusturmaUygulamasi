<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="KullaniciYonetimi.aspx.cs" Inherits="ExamApp.KullaniciYonetimi" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Kullanıcı Yönetimi</h2>
    <asp:GridView ID="gvKullanici" runat="server" AutoGenerateColumns="False" DataKeyNames="KullaniciID" DataSourceID="sqlKullanici">
        <Columns>
            <asp:BoundField DataField="KullaniciID" HeaderText="ID" ReadOnly="True" SortExpression="KullaniciID" />
            <asp:BoundField DataField="KullaniciAdi" HeaderText="Kullanıcı Adı" SortExpression="KullaniciAdi" />
            <asp:ButtonField CommandName="Delete" Text="Sil" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sqlKullanici" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr %>" SelectCommand="SELECT * FROM Kullanici" DeleteCommand="DELETE FROM Kullanici WHERE KullaniciID = @KullaniciID">
        <DeleteParameters>
            <asp:Parameter Name="KullaniciID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <h3>Yeni Kullanıcı Ekle</h3>
    <asp:TextBox ID="txtYeniKullaniciAdi" runat="server" Placeholder="Kullanıcı Adı"></asp:TextBox>
    <asp:TextBox ID="txtYeniSifre" runat="server" TextMode="Password" Placeholder="Şifre"></asp:TextBox>
    <asp:Button ID="btnYeniKullaniciEkle" runat="server" Text="Ekle" OnClick="btnYeniKullaniciEkle_Click" />
</asp:Content>
