<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="SinavYonetimi.aspx.cs" Inherits="ExamApp.SinavYonetimi" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Yeni Sınav Ekle</h2>
    <asp:Label ID="lblDersSec" runat="server" Text="Ders Seçin: "></asp:Label>
    <asp:DropDownList ID="ddlDersler" runat="server"></asp:DropDownList>
    <br />
    <asp:TextBox ID="txtSinavAdi" runat="server" Placeholder="Sınav Adı"></asp:TextBox>
    <br />
    <asp:Button ID="btnSinavEkle" runat="server" Text="Sınav Ekle" OnClick="btnSinavEkle_Click" />
    <asp:Label ID="lblMesaj" runat="server" ForeColor="Green"></asp:Label>
    <br /><br />
    <asp:GridView ID="gvSinavlar" runat="server" AutoGenerateColumns="False" DataKeyNames="SinavID" DataSourceID="sqlSinavlar">
        <Columns>
            <asp:BoundField DataField="SinavID" HeaderText="ID" ReadOnly="True" SortExpression="SinavID" />
            <asp:BoundField DataField="SinavAdi" HeaderText="Sınav Adı" SortExpression="SinavAdi" />
            <asp:BoundField DataField="OlusturulmaTarihi" HeaderText="Oluşturulma Tarihi" SortExpression="OlusturulmaTarihi" />
            <asp:BoundField DataField="DersAdi" HeaderText="Ders Adı" SortExpression="DersAdi" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sqlSinavlar" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr %>" 
        SelectCommand="SELECT Sinav.SinavID, Sinav.SinavAdi, Sinav.OlusturulmaTarihi, Ders.DersAdi FROM Sinav INNER JOIN Ders ON Sinav.DersID = Ders.DersID" 
        DeleteCommand="DELETE FROM Sinav WHERE SinavID = @SinavID">
        <DeleteParameters>
            <asp:Parameter Name="SinavID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
