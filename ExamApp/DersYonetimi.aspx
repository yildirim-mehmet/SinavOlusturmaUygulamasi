<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="DersYonetimi.aspx.cs" Inherits="ExamApp.DersYonetimi" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Ders Yönetimi</h2>
    <asp:GridView ID="gvDers" runat="server" AutoGenerateColumns="False" DataKeyNames="DersID" DataSourceID="sqlDers">
        <Columns>
            <asp:BoundField DataField="DersID" HeaderText="ID" ReadOnly="True" SortExpression="DersID" />
            <asp:BoundField DataField="DersAdi" HeaderText="Ders Adı" SortExpression="DersAdi" />
            <asp:ButtonField CommandName="Delete" Text="Sil" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sqlDers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr %>" 
        SelectCommand="SELECT * FROM Ders" DeleteCommand="DELETE FROM Ders WHERE DersID = @DersID">
        <DeleteParameters>
            <asp:Parameter Name="DersID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <h3>Yeni Ders Ekle</h3>
    <asp:TextBox ID="txtYeniDersAdi" runat="server" Placeholder="Ders Adı"></asp:TextBox>
    <asp:Button ID="btnYeniDersEkle" runat="server" Text="Ekle" OnClick="btnYeniDersEkle_Click" />
</asp:Content>
