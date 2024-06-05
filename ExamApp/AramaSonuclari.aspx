<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="AramaSonuclari.aspx.cs" Inherits="ExamApp.AramaSonuclari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Arama Sonuçları</h2>
    <asp:GridView ID="gvSonuclar" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="SoruID" HeaderText="Soru ID" />
            <asp:BoundField DataField="KonuID" HeaderText="Konu ID" />
            <asp:BoundField DataField="SoruMetni" HeaderText="Soru Metni" />
            <asp:BoundField DataField="ZorlukSeviyesi" HeaderText="Zorluk Seviyesi" />
        </Columns>
    </asp:GridView>
</asp:Content>
