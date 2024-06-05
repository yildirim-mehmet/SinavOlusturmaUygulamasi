<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="SinavOlusturma.aspx.cs" Inherits="ExamApp.SinavOlusturma1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <title>Sınav Oluşturma</title>
        <h2>Sınav Oluşturma</h2>
    <asp:DropDownList ID="ddlDersler" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDersler_SelectedIndexChanged">
    </asp:DropDownList>
    <br />
    <asp:DropDownList ID="ddlKonular" runat="server">
    </asp:DropDownList>
    <br />
    <asp:TextBox ID="txtSoruSayisi" runat="server" Placeholder="Soru Sayısı"></asp:TextBox>
    <br />
    <asp:TextBox ID="txtSikSayisi" runat="server" Placeholder="Sorulardaki Şık Sayısı"></asp:TextBox>
    <br />
    <asp:CheckBox ID="chkZorSorular" runat="server" Text="Zor Sorulardan Sor" />
    <br />
    <asp:Button ID="btnSinavOlustur" runat="server" Text="Sınav Oluştur" OnClick="btnSinavOlustur_Click" />
    <br />
    <asp:Label ID="lblMesaj" runat="server"></asp:Label>
</asp:Content>


<%--<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sınav Oluşturma</h2>
    <asp:Label ID="lblDersSec" runat="server" Text="Ders Seçin: "></asp:Label>
    <asp:DropDownList ID="ddlDersler" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDersler_SelectedIndexChanged"></asp:DropDownList>
    <br />
    <asp:Label ID="lblKonuSec" runat="server" Text="Konu Seçin: "></asp:Label>
    <asp:DropDownList ID="ddlKonular" runat="server"></asp:DropDownList>
    <br />
    <asp:TextBox ID="txtSoruSayisi" runat="server" Placeholder="Soru Sayısı"></asp:TextBox>
    <br />
    <asp:Button ID="btnSinavOlustur" runat="server" Text="Sınav Oluştur" OnClick="btnSinavOlustur_Click" />
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
</asp:Content>--%>
