<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="KonuYonetimi.aspx.cs" Inherits="ExamApp.KonuYonetimi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Konu Yönetimi</h2>
    <asp:GridView ID="gvKonu" runat="server" AutoGenerateColumns="False" DataKeyNames="KonuID" DataSourceID="sqlKonu">
        <Columns>
            <asp:BoundField DataField="KonuID" HeaderText="ID" ReadOnly="True" SortExpression="KonuID" />
            <asp:BoundField DataField="KonuAdi" HeaderText="Konu Adı" SortExpression="KonuAdi" />
            <asp:BoundField DataField="DersAdi" HeaderText="Ders Adı" SortExpression="DersAdi" />
            <asp:ButtonField CommandName="Delete" Text="Sil" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sqlKonu" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr %>" 
        SelectCommand="SELECT K.KonuID, K.KonuAdi, D.DersAdi FROM Konu K JOIN Ders D ON K.DersID = D.DersID" 
        DeleteCommand="DELETE FROM Konu WHERE KonuID = @KonuID">
        <DeleteParameters>
            <asp:Parameter Name="KonuID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <h3>Yeni Konu Ekle</h3>
    <asp:DropDownList ID="ddlDers" runat="server" DataSourceID="sqlDers" DataTextField="DersAdi" DataValueField="DersID" />
    <asp:SqlDataSource ID="sqlDers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr %>" SelectCommand="SELECT * FROM Ders"></asp:SqlDataSource>
    <asp:TextBox ID="txtYeniKonuAdi" runat="server" Placeholder="Konu Adı"></asp:TextBox>
    <asp:Button ID="btnYeniKonuEkle" runat="server" Text="Ekle" OnClick="btnYeniKonuEkle_Click" />
</asp:Content>
