<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="SoruEkleme.aspx.cs" Inherits="ExamApp.SoruEkleme" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Soru Ekleme</h2>
    <asp:Label ID="lblDersSec" runat="server" Text="Ders Seçin: "></asp:Label>
    <asp:DropDownList ID="ddlDersler" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDersler_SelectedIndexChanged"></asp:DropDownList>
    <br />
    <asp:Label ID="lblKonuSec" runat="server" Text="Konu Seçin: "></asp:Label>
    <asp:DropDownList ID="ddlKonular" runat="server"></asp:DropDownList>
    <br />
    <asp:TextBox ID="txtSoruMetni" runat="server" TextMode="MultiLine" Rows="5" Placeholder="Soru Metni"></asp:TextBox>
    <br />
    <asp:CheckBox ID="chkZorSoru" runat="server" Text="Zor Soru mu?" />
    <br />
    <asp:Button ID="btnSoruEkle" runat="server" Text="Soru Ekle" OnClick="btnSoruEkle_Click" />
    <asp:Label ID="lblMesaj" runat="server" ForeColor="Green"></asp:Label>
    <br /><br />
    <asp:GridView ID="gvSorular" runat="server" AutoGenerateColumns="False" DataKeyNames="SoruID" DataSourceID="sqlSorular">
        <Columns>
            <asp:BoundField DataField="SoruID" HeaderText="ID" ReadOnly="True" SortExpression="SoruID" />
            <asp:BoundField DataField="SoruMetni" HeaderText="Soru Metni" SortExpression="SoruMetni" />
            <asp:CheckBoxField DataField="ZorlukSeviyesi" HeaderText="Zor Soru" SortExpression="ZorlukSeviyesi" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sqlSorular" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr %>" 
        SelectCommand="SELECT * FROM Soru WHERE KonuID = @KonuID" 
        DeleteCommand="DELETE FROM Soru WHERE SoruID = @SoruID">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlKonular" Name="KonuID" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="SoruID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
