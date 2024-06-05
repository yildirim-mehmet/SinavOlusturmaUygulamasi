<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfa.Master" AutoEventWireup="true" CodeBehind="AnaSayfa.aspx.cs" Inherits="ExamApp.AnaSayfa1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Hoşgeldiniz, <%= User.Identity.Name %>!</h2>
</asp:Content>
