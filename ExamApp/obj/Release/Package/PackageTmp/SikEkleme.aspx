<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="SikEkleme.aspx.cs" Inherits="ExamApp.SikEkleme" %>




<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Şık Ekleme</h2>
    <asp:Label ID="lblSoruSec" runat="server" Text="Soru Seçin: "></asp:Label>
    <asp:DropDownList ID="ddlSorular" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSorular_SelectedIndexChanged"></asp:DropDownList>
    <br />
    <asp:TextBox ID="txtSikMetni" runat="server" Placeholder="Şık Metni"></asp:TextBox>
    <asp:CheckBox ID="chkDogruMu" runat="server" Text="Doğru Şık mı?" />
    <asp:Button ID="btnSikEkle" runat="server" Text="Şık Ekle" OnClick="btnSikEkle_Click" />
    <asp:Label ID="lblMesaj" runat="server" ForeColor="Green"></asp:Label>
    <br /><br />
    <asp:GridView ID="gvSiklar" runat="server" AutoGenerateColumns="False" DataKeyNames="SikID" DataSourceID="sqlSiklar">
        <Columns>
            <asp:BoundField DataField="SikID" HeaderText="ID" ReadOnly="True" SortExpression="SikID" />
            <asp:BoundField DataField="SikMetni" HeaderText="Şık Metni" SortExpression="SikMetni" />
            <asp:CheckBoxField DataField="DogruMu" HeaderText="Doğru Şık" SortExpression="DogruMu" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sqlSiklar" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr %>" 
        SelectCommand="SELECT * FROM Sik WHERE SoruID = @SoruID" 
        DeleteCommand="DELETE FROM Sik WHERE SikID = @SikID">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlSorular" Name="SoruID" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="SikID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
